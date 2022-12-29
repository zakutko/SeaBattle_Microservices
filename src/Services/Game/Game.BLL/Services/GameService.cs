using AutoMapper;
using Game.BLL.Interfaces;
using Game.DAL.Interfaces;
using Game.DAL.Models;
using SeaBattle.Contracts.Dtos;

namespace Game.BLL.Services
{
    public class GameService : IGameService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGameServiceHelper _gameServiceHelper;
        private readonly IMapper _mapper;

        public GameService(IUnitOfWork unitOfWork, IGameServiceHelper gameServiceHelper, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _gameServiceHelper = gameServiceHelper;
            _mapper = mapper;
        }

        public IEnumerable<GameListResponse> GetAllGames(GameListRequest gameListRequest)
        {
            var playerGameList = _unitOfWork.PlayerGameRepository.GetAllAsync().Result;

            var playerGameResponseList = new List<GameListResponse>();

            foreach (var playerGame in playerGameList)
            {
                var game = _unitOfWork.GameRepository.GetAsync(playerGame.GameId).Result;
                var firstPlayer = _unitOfWork.AppUserRepository.GetAsync(playerGame.FirstPlayerId).Result;
                var secondPlayer = _unitOfWork.AppUserRepository.GetAsync(playerGame.SecondPlayerId).Result;
                var gameState = _unitOfWork.GameStateRepository.GetAsync(game.GameStateId).Result.GameStateName;

                var numberOfPlayers = 2;
                if (secondPlayer == null)
                {
                    numberOfPlayers = 1;
                }

                playerGameResponseList.Add(new GameListResponse
                {
                    Id = game.Id,
                    FirstPlayer = firstPlayer.UserName,
                    SecondPlayer = secondPlayer?.UserName,
                    GameState = gameState,
                    NumberOfPlayers = numberOfPlayers
                });
            }

            var username = _gameServiceHelper.GetUsernameByDecodingJwtToken(gameListRequest.Token);

            var playerGameResponseListWithoutCurrUser = playerGameResponseList.Where(playerGame => playerGame.FirstPlayer != username && playerGame.SecondPlayer != username);
            return playerGameResponseListWithoutCurrUser;
        }

        public void CreateGame(CreateGameRequest createGameRequest)
        {
            var game = new DAL.Models.Game { GameStateId = 1 };
            _unitOfWork.GameRepository.Create(game);
            _unitOfWork.Commit();
            var gameId = game.Id;

            var username = _gameServiceHelper.GetUsernameByDecodingJwtToken(createGameRequest.Token);
            var appUser = _unitOfWork.AppUserRepository.GetAsync(x => x.UserName == username && x.NormalizedUserName == username.ToUpper()).Result;
            var newAppUser = _gameServiceHelper.CreateNewAppUser(appUser, true);

            _unitOfWork.ClearChangeTracker();
            _unitOfWork.AppUserRepository.Update(newAppUser);
            _unitOfWork.Commit();

            var playerGame = new PlayerGame { GameId = gameId, FirstPlayerId = appUser.Id };
            _unitOfWork.PlayerGameRepository.Create(playerGame);
            _unitOfWork.Commit();

            var field = new Field { Size = 10, PlayerId = appUser.Id };
            _unitOfWork.FieldRepository.Create(field);
            _unitOfWork.Commit();
            var fieldId = field.Id;

            var gameField = new GameField { FirstFieldId = fieldId, GameId = gameId };
            _unitOfWork.GameFieldRepository.Create(gameField);
            _unitOfWork.Commit();

            var numberOfShipsOnField = _unitOfWork.ShipWrapperRepository.GetAllAsync(x => x.FieldId == fieldId && x.ShipId != null).Result.Count();
            if (numberOfShipsOnField == 0)
            {
                var defaultCells = _gameServiceHelper.SetDafaultCells();
                _unitOfWork.ClearChangeTracker();
                _unitOfWork.CellRepository.UpdateRange(defaultCells);
                _unitOfWork.Commit();

                var defaultShipWrapper = new ShipWrapper { FieldId = fieldId };
                _unitOfWork.ShipWrapperRepository.Create(defaultShipWrapper);
                _unitOfWork.Commit();

                var defaultPositions = defaultCells.Select(cell => new Position { ShipWrapperId = defaultShipWrapper.Id, CellId = cell.Id });
                _unitOfWork.PositionRepository.CreateRange(defaultPositions);
                _unitOfWork.Commit();
            }
        }

        public IsGameOwnerResponse IsGameOwner(IsGameOwnerRequest isGameOwnerRequest)
        {
            var username = _gameServiceHelper.GetUsernameByDecodingJwtToken(isGameOwnerRequest.Token);
            var playerId = _unitOfWork.AppUserRepository.GetAsync(x => x.UserName == username).Result.Id;

            var playerGame = _unitOfWork.PlayerGameRepository.GetAsync(x => x.FirstPlayerId == playerId || x.SecondPlayerId == playerId).Result;

            if (playerGame == null)
            {
                return new IsGameOwnerResponse
                {
                    IsGameOwner = false,
                    IsSecondPlayerConnected = false
                };
            }

            var isGameOwner = false;
            if (playerGame.FirstPlayerId == playerId)
            {
                isGameOwner = true;
            }

            var isSecondPlayerConnected = true;
            if (playerGame.FirstPlayerId == playerId && playerGame.SecondPlayerId == null)
            {
                isSecondPlayerConnected = false;
            }

            return new IsGameOwnerResponse
            {
                IsGameOwner = isGameOwner,
                IsSecondPlayerConnected = isSecondPlayerConnected
            };
        }

        public void DeleteGame(DeleteGameRequest deleteGameRequest)
        {
            var username = _gameServiceHelper.GetUsernameByDecodingJwtToken(deleteGameRequest.Token);
            var firstPlayerId = _unitOfWork.AppUserRepository.GetAsync(x => x.UserName == username).Result.Id;

            var gameId = _unitOfWork.PlayerGameRepository.GetAsync(x => x.FirstPlayerId == firstPlayerId && x.SecondPlayerId == null).Result.GameId;
            var fieldId = _unitOfWork.FieldRepository.GetAsync(x => x.PlayerId == firstPlayerId).Result.Id;
            var shipWrappers = _unitOfWork.ShipWrapperRepository.GetAllAsync(x => x.FieldId == fieldId).Result;
            var ships = _gameServiceHelper.GetShipList(fieldId).Result;
            var cellList = _gameServiceHelper.GetCellList(fieldId).Result;

            if (cellList.Any())
            {
                //delete all shipWrappers
                _unitOfWork.ShipWrapperRepository.DeleteRange(shipWrappers);
                _unitOfWork.Commit();

                //delete all ships
                _unitOfWork.ShipRepository.DeleteRange(ships);
                _unitOfWork.Commit();

                //delete all cells
                _unitOfWork.CellRepository.DeleteRange(cellList);
                _unitOfWork.Commit();

                //update appUser
                var appUser = _unitOfWork.AppUserRepository.GetAsync(firstPlayerId).Result;
                var newAppUser = _gameServiceHelper.CreateNewAppUser(appUser, null);
                _unitOfWork.ClearChangeTracker();
                _unitOfWork.AppUserRepository.Update(newAppUser);
                _unitOfWork.Commit();

                //delete game from table Game
                var game = _unitOfWork.GameRepository.GetAsync(x => x.Id == gameId).Result;
                _unitOfWork.GameRepository.Delete(game);
                _unitOfWork.Commit();

                //delete field from table Field
                var field = _unitOfWork.FieldRepository.GetAsync(x => x.Id == fieldId).Result;
                _unitOfWork.FieldRepository.Delete(field);
                _unitOfWork.Commit();
            }
            else
            {
                //delete game from table Game
                _unitOfWork.GameRepository.Delete(_unitOfWork.GameRepository.GetAsync(gameId).Result);
                _unitOfWork.Commit();

                //delete field from table Field
                _unitOfWork.FieldRepository.Delete(_unitOfWork.FieldRepository.GetAsync(fieldId).Result);
                _unitOfWork.Commit();

                //update appUser
                var appUser = _unitOfWork.AppUserRepository.GetAsync(firstPlayerId).Result;
                var newAppUser = _gameServiceHelper.CreateNewAppUser(appUser, null);
                _unitOfWork.ClearChangeTracker();
                _unitOfWork.AppUserRepository.Update(newAppUser);
                _unitOfWork.Commit();
            }
        }

        public void JoinSecondPlayer(JoinSecondPlayerRequest joinSecondPlayerRequest)
        {
            var username = _gameServiceHelper.GetUsernameByDecodingJwtToken(joinSecondPlayerRequest.Token);

            var secondPlayerId = _unitOfWork.AppUserRepository.GetAsync(x => x.UserName == username).Result.Id;
            var firstPlayerId = _unitOfWork.PlayerGameRepository.GetAsync(x => x.GameId == joinSecondPlayerRequest.GameId).Result.FirstPlayerId;

            var playerGame = _unitOfWork.PlayerGameRepository.GetAsync(x => x.FirstPlayerId == firstPlayerId && x.SecondPlayerId == null).Result;

            var firstFieldId = _unitOfWork.GameFieldRepository.GetAsync(x => x.GameId == joinSecondPlayerRequest.GameId).Result.FirstFieldId;
            var gameFieldId = _unitOfWork.GameFieldRepository.GetAsync(x => x.GameId == joinSecondPlayerRequest.GameId && x.FirstFieldId == firstFieldId).Result.Id;

            //update table AppUser
            var appUser = _unitOfWork.AppUserRepository.GetAsync(secondPlayerId).Result;
            var newAppUser = _gameServiceHelper.CreateNewAppUser(appUser, false);
            _unitOfWork.ClearChangeTracker();
            _unitOfWork.AppUserRepository.Update(newAppUser);
            _unitOfWork.Commit();

            //update table Game
            var game = _unitOfWork.GameRepository.GetAsync(joinSecondPlayerRequest.GameId).Result;
            var newGame = new DAL.Models.Game
            {
                Id = game.Id,
                GameStateId = 2
            };
            _unitOfWork.ClearChangeTracker();
            _unitOfWork.GameRepository.Update(newGame);
            _unitOfWork.Commit();

            //update table Field
            var newField = new Field()
            {
                Size = 10,
                PlayerId = secondPlayerId
            };
            _unitOfWork.ClearChangeTracker();
            _unitOfWork.FieldRepository.Create(newField);
            _unitOfWork.Commit();
            var fieldId = newField.Id;

            //update table PlayerGame
            var newPlayerGame = new PlayerGame()
            {
                Id = playerGame.Id,
                GameId = playerGame.GameId,
                FirstPlayerId = playerGame.FirstPlayerId,
                SecondPlayerId = secondPlayerId,
                IsReadyFirstPlayer = playerGame.IsReadyFirstPlayer,
                IsReadySecondPlayer = playerGame.IsReadySecondPlayer
            };
            _unitOfWork.ClearChangeTracker();
            _unitOfWork.PlayerGameRepository.Update(newPlayerGame);
            _unitOfWork.Commit();

            //update table GameField
            var newGameField = new GameField()
            {
                Id = gameFieldId,
                FirstFieldId = firstFieldId,
                SecondFieldId = newField.Id,
                GameId = joinSecondPlayerRequest.GameId
            };
            _unitOfWork.ClearChangeTracker();
            _unitOfWork.GameFieldRepository.Update(newGameField);
            _unitOfWork.Commit();

            var numberOfShipsOnField = _unitOfWork.ShipWrapperRepository.GetAllAsync(x => x.FieldId == fieldId && x.ShipId != null).Result.Count();
            if (numberOfShipsOnField == 0)
            {
                var defaultCells = _gameServiceHelper.SetDafaultCells();
                _unitOfWork.ClearChangeTracker();
                _unitOfWork.CellRepository.UpdateRange(defaultCells);
                _unitOfWork.Commit();

                var defaultShipWrapper = new ShipWrapper { FieldId = fieldId };
                _unitOfWork.ShipWrapperRepository.Create(defaultShipWrapper);
                _unitOfWork.Commit();

                var defaultPositions = (defaultCells.Select(cell => new Position { ShipWrapperId = defaultShipWrapper.Id, CellId = cell.Id })).ToList();
                _unitOfWork.PositionRepository.CreateRange(defaultPositions);
                _unitOfWork.Commit();
            }
        }

        public IEnumerable<CellListResponse> GetAllCells(CellListRequest cellListRequest)
        {
            var username = _gameServiceHelper.GetUsernameByDecodingJwtToken(cellListRequest.Token);
            var playerId = _unitOfWork.AppUserRepository.GetAsync(x => x.UserName == username).Result.Id;

            var fieldId = _unitOfWork.FieldRepository.GetAsync(x => x.PlayerId == playerId).Result.Id;
            var cellList = _gameServiceHelper.GetCellList(fieldId).Result.OrderBy(x => x.Id);
            return cellList.Select(_mapper.Map<CellListResponse>);
        }

        public CreateShipResponse CreateShipOnField(CreateShipRequest createShipRequest)
        {
            var username = _gameServiceHelper.GetUsernameByDecodingJwtToken(createShipRequest.Token);
            var playerId = _unitOfWork.AppUserRepository.GetAsync(x => x.UserName == username).Result.Id;

            var fieldId = _unitOfWork.FieldRepository.GetAsync(x => x.PlayerId == playerId).Result.Id;

            var numberOfShipsOnField = _unitOfWork.ShipWrapperRepository.GetAllAsync(x => x.FieldId == fieldId && x.ShipId != null).Result.Count();
            if (numberOfShipsOnField == 10)
            {
                return new CreateShipResponse { Message = "There are already 10 ships on the field!" };
            }

            var shipWrappers = _unitOfWork.ShipWrapperRepository.GetAllAsync(x => x.FieldId == fieldId).Result;

            var ships = shipWrappers.SelectMany(shipWrapperItem => _unitOfWork.ShipRepository.GetAllAsync(x => x.Id == shipWrapperItem.ShipId).Result);

            switch (createShipRequest.ShipSize)
            {
                case 1:
                    var numberOfShipsWhereSizeOne = ships.Where(x => x.ShipSizeId == 1).Count();
                    if (numberOfShipsWhereSizeOne == 4)
                    {
                        return new CreateShipResponse { Message = "The maximum number of ships with the size 1 on the field is 4!" };
                    }
                    break;
                case 2:
                    var numberOfShipsWhereSizeTwo = ships.Where(x => x.ShipSizeId == 2).Count();
                    if (numberOfShipsWhereSizeTwo == 3)
                    {
                        return new CreateShipResponse { Message = "The maximum number of ships with the size 2 on the field is 3!" };
                    }
                    break;
                case 3:
                    var numberOfShipsWhereSizeThree = ships.Where(x => x.ShipSizeId == 3).Count();
                    if (numberOfShipsWhereSizeThree == 2)
                    {
                        return new CreateShipResponse { Message = "The maximum number of ships with the size 3 on the field is 2!" };
                    }
                    break;
                case 4:
                    var numberOfShipsWhereSizeFour = ships.Where(x => x.ShipSizeId == 4).Count();
                    if (numberOfShipsWhereSizeFour == 1)
                    {
                        return new CreateShipResponse { Message = "The maximum number of ships with the size 4 on the field is 1!" };
                    }
                    break;
            }

            //add ship to Ship table
            var newShip = new Ship { DirectionId = createShipRequest.ShipDirection, ShipStateId = 1, ShipSizeId = createShipRequest.ShipSize };
            _unitOfWork.ShipRepository.Create(newShip);

            //add shipWrapper to ShipWrapper table
            var shipWrapper = new ShipWrapper { ShipId = newShip.Id, FieldId = fieldId };
            _unitOfWork.ShipWrapperRepository.Create(shipWrapper);
            _unitOfWork.Commit();

            var shipDirectionName = _unitOfWork.DirectionRepository.GetAsync(createShipRequest.ShipDirection).Result.DirectionName;
            try
            {
                var cellListResult = _gameServiceHelper.GetAllCells(shipDirectionName, createShipRequest.ShipSize, createShipRequest.X, createShipRequest.Y, fieldId).Result;
                if (!cellListResult.Any())
                {
                    _unitOfWork.ShipWrapperRepository.Delete(shipWrapper);
                    _unitOfWork.ShipRepository.Delete(newShip);
                    _unitOfWork.Commit();
                }

                var cells = _gameServiceHelper.GetCellList(fieldId).Result;

                foreach (var cell in cellListResult)
                {
                    var defaultCell = cells.Where(x => x.X == cell.X && x.Y == cell.Y).First();

                    if (defaultCell.CellStateId == 2)
                    {
                       throw new Exception("One of Cells is busy!");
                    }
                    else if(defaultCell.CellStateId == 5)
                    {
                        continue;
                    }
                    else
                    {
                        _unitOfWork.ClearChangeTracker();
                        _unitOfWork.CellRepository.Update(new Cell { Id = defaultCell.Id, X = cell.X, Y = cell.Y, CellStateId = cell.CellStateId });

                        //update positions in Position table
                        var positionId = _unitOfWork.PositionRepository.GetAsync(x => x.CellId == defaultCell.Id).Result.Id;
                        var updatePosition = new Position
                        {
                            Id = positionId,
                            ShipWrapperId = shipWrapper.Id,
                            CellId = defaultCell.Id
                        };

                        _unitOfWork.PositionRepository.Update(updatePosition);
                        _unitOfWork.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                _unitOfWork.ShipWrapperRepository.Delete(shipWrapper);
                _unitOfWork.ShipRepository.Delete(newShip);
                _unitOfWork.Commit();
                return new CreateShipResponse { Message = ex.Message };
            }

            return new CreateShipResponse { Message = "Create ship was successful!" };
        }
        
        public IsPlayerReadyResponse SetPlayerReady(IsPlayerReadyRequest isPlayerReadyRequest)
        {
            var username = _gameServiceHelper.GetUsernameByDecodingJwtToken(isPlayerReadyRequest.Token);
            var playerId = _unitOfWork.AppUserRepository.GetAsync(x => x.UserName == username).Result.Id;

            var fieldId = _unitOfWork.FieldRepository.GetAsync(x => x.PlayerId == playerId).Result.Id;
            var shipWrappers = _unitOfWork.ShipWrapperRepository.GetAllAsync(x => x.FieldId == fieldId).Result;

            if (shipWrappers.Count() < 11)
            {
                return new IsPlayerReadyResponse { Message = "Number of ships must be 10!" };
            }

            var playerGame = _unitOfWork.PlayerGameRepository.GetAsync(x => x.FirstPlayerId == playerId || x.SecondPlayerId == playerId).Result;
            if (playerGame.IsReadyFirstPlayer != null)
            {
                var newPlayerGame = new PlayerGame
                {
                    Id = playerGame.Id,
                    GameId = playerGame.GameId,
                    FirstPlayerId = playerGame.FirstPlayerId,
                    SecondPlayerId = playerGame.SecondPlayerId,
                    IsReadyFirstPlayer = true,
                    IsReadySecondPlayer = true
                };

                _unitOfWork.ClearChangeTracker();
                _unitOfWork.PlayerGameRepository.Update(newPlayerGame);
                _unitOfWork.Commit();

                return new IsPlayerReadyResponse { Message = "The Player is ready!" };
            }
            else
            {
                var newPlayerGame = new PlayerGame
                {
                    Id = playerGame.Id,
                    GameId = playerGame.GameId,
                    FirstPlayerId = playerGame.FirstPlayerId,
                    SecondPlayerId = playerGame.SecondPlayerId,
                    IsReadyFirstPlayer = true,
                    IsReadySecondPlayer = playerGame.IsReadySecondPlayer
                };

                _unitOfWork.ClearChangeTracker();
                _unitOfWork.PlayerGameRepository.Update(newPlayerGame);
                _unitOfWork.Commit();

                return new IsPlayerReadyResponse { Message = "The Player is ready!" };
            }
        }
        
        public IsTwoPlayersReadyResponse IsTwoPlayersReady(IsTwoPlayersReadyRequest isTwoPlayersReadyRequest)
        {
            var username = _gameServiceHelper.GetUsernameByDecodingJwtToken(isTwoPlayersReadyRequest.Token);
            var playerId = _unitOfWork.AppUserRepository.GetAsync(x => x.UserName == username).Result.Id;
            
            var playerGame = _unitOfWork.PlayerGameRepository.GetAsync(x => x.FirstPlayerId == playerId || x.SecondPlayerId == playerId).Result;

            var numberOfReadyPlayers = 0;
            if (playerGame.IsReadyFirstPlayer == null && playerGame.IsReadySecondPlayer == null)
            {
                numberOfReadyPlayers = 0;
            }
            else if (playerGame.IsReadyFirstPlayer != null && playerGame.IsReadySecondPlayer == null)
            {
                numberOfReadyPlayers = 1;
            }
            else
            {
                numberOfReadyPlayers = 2;
            }
            return new IsTwoPlayersReadyResponse
            {
                NumberOfReadyPlayers = numberOfReadyPlayers
            };
        }
        
        public IEnumerable<CellListResponseForSecondPlayer> GetAllCellForSecondPlayer(CellListRequestForSecondPlayer cellListRequestForSecondPlayer)
        {
            var username = _gameServiceHelper.GetUsernameByDecodingJwtToken(cellListRequestForSecondPlayer.Token);
            var playerId = _unitOfWork.AppUserRepository.GetAsync(x => x.UserName == username).Result.Id;

            var secondPlayerId = _gameServiceHelper.GetSecondPlayerId(playerId).Result;

            if (secondPlayerId == null)
            {
                return Enumerable.Empty<CellListResponseForSecondPlayer>();
            }

            var fieldId = _unitOfWork.FieldRepository.GetAsync(x => x.PlayerId == secondPlayerId).Result.Id;
            var cellList = _gameServiceHelper.GetCellList(fieldId).Result.OrderBy(x => x.Id);

            return cellList.Select(_mapper.Map<CellListResponseForSecondPlayer>);
        }
        
        public ShootResponse Fire(ShootRequest shootRequest)
        {
            var username = _gameServiceHelper.GetUsernameByDecodingJwtToken(shootRequest.Token);
            var playerId = _unitOfWork.AppUserRepository.GetAsync(x => x.UserName == username).Result.Id;
            var secondPlayerId = _gameServiceHelper.GetSecondPlayerId(playerId).Result;

            var fieldId = _unitOfWork.FieldRepository.GetAsync(x => x.PlayerId == secondPlayerId).Result.Id;
            var cellList = _gameServiceHelper.GetCellList(fieldId).Result;

            var myCell = cellList.Where(x => x.X == shootRequest.X && x.Y == shootRequest.Y).FirstOrDefault();

            if (myCell.CellStateId == 1 || myCell.CellStateId == 5)
            {
                _unitOfWork.ClearChangeTracker();
                _unitOfWork.CellRepository.Update(_gameServiceHelper.CreateNewCell(myCell.Id, myCell.X, myCell.Y, myCell.CellStateId, false));
                _unitOfWork.Commit();

                var appUser = _unitOfWork.AppUserRepository.GetAsync(x => x.Id == playerId).Result;
                var newAppUser = _gameServiceHelper.CreateNewAppUser(appUser, false);
                _unitOfWork.ClearChangeTracker();
                _unitOfWork.AppUserRepository.Update(newAppUser);
                _unitOfWork.Commit();

                var secondAppUser = _unitOfWork.AppUserRepository.GetAsync(x => x.Id == secondPlayerId).Result;
                var newSecondAppUser = _gameServiceHelper.CreateNewAppUser(secondAppUser, true);
                _unitOfWork.ClearChangeTracker();
                _unitOfWork.AppUserRepository.Update(newSecondAppUser);
                _unitOfWork.Commit();

                return new ShootResponse { Message = "Missed the fire!" };
            }
            else
            {
                var shipWrapperId = _unitOfWork.PositionRepository.GetAsync(x => x.CellId == myCell.Id).Result.ShipWrapperId;
                var positionsByShipWrapperId = _unitOfWork.PositionRepository.GetAllAsync(x => x.ShipWrapperId == shipWrapperId).Result;
                var cellsByCellIds = positionsByShipWrapperId.Select(position => _unitOfWork.CellRepository.GetAsync(position.CellId).Result);
                var isDestroyed = cellsByCellIds.Count(x => x.CellStateId == 2) <= 1;

                var newCell = _gameServiceHelper.CreateNewCell(myCell.Id, myCell.X, myCell.Y, myCell.CellStateId, false);

                _unitOfWork.ClearChangeTracker();
                _unitOfWork.CellRepository.Update(newCell);
                _unitOfWork.Commit();

                if (isDestroyed)
                {
                    foreach (var cellByCellId in cellsByCellIds)
                    {
                        var newCellByCellId = _gameServiceHelper.CreateNewCell(cellByCellId.Id, cellByCellId.X, cellByCellId.Y, cellByCellId.CellStateId, isDestroyed);

                        _unitOfWork.ClearChangeTracker();
                        _unitOfWork.CellRepository.Update(newCellByCellId);
                        _unitOfWork.Commit();
                    }

                    return new ShootResponse { Message = "The ship is destroyed!" };
                }
                return new ShootResponse { Message = "The ship is hit!" };
            }
        }
        
        public HitResponse GetPriority(HitRequest hitRequest)
        {
            var username = _gameServiceHelper.GetUsernameByDecodingJwtToken(hitRequest.Token);
            var playerId = _unitOfWork.AppUserRepository.GetAsync(x => x.UserName == username).Result.Id;
            var player = _unitOfWork.AppUserRepository.GetAsync(playerId).Result;

            return new HitResponse { IsHit = player.IsHit };
        }
        //TODO: Need to speed up
        public IsEndOfTheGameResponse IsEndOfTheGame(IsEndOfTheGameRequest isEndOfTheGameRequest)
        {
            var username = _gameServiceHelper.GetUsernameByDecodingJwtToken(isEndOfTheGameRequest.Token);
            var firstPlayerId = _unitOfWork.AppUserRepository.GetAsync(x => x.UserName == username).Result.Id;
            var firstFieldId = _unitOfWork.FieldRepository.GetAsync(x => x.PlayerId == firstPlayerId).Result.Id;
            var firstCellList = _gameServiceHelper.GetCellList(firstFieldId).Result;

            var gameId = _unitOfWork.PlayerGameRepository.GetAsync(x => x.FirstPlayerId == firstPlayerId || x.SecondPlayerId == firstPlayerId).Result.GameId;

            var secondPlayerId = _gameServiceHelper.GetSecondPlayerId(firstPlayerId).Result;
            if (secondPlayerId == null)
            {
                return new IsEndOfTheGameResponse { IsEndOfTheGame = false, WinnerUserName = "" };
            }
            var secondFieldId = _unitOfWork.FieldRepository.GetAsync(x => x.PlayerId == secondPlayerId).Result.Id;
            var secondCellList = _gameServiceHelper.GetCellList(secondFieldId).Result;

            var gameStateId = _unitOfWork.GameRepository.GetAsync(gameId).Result.GameStateId;
            var firstCellsWithStateBusyOrHit = _gameServiceHelper.CheckIsCellsWithStateBusyOrHit(firstCellList, gameStateId);
            var secondCellsWithStateBusyOrHit = _gameServiceHelper.CheckIsCellsWithStateBusyOrHit(secondCellList, gameStateId);

            if (secondCellsWithStateBusyOrHit && firstCellsWithStateBusyOrHit && _unitOfWork.GameRepository.GetAsync(gameId).Result.GameStateId == 2)
            {
                return new IsEndOfTheGameResponse { IsEndOfTheGame = false, WinnerUserName = "" };
            }

            var game = _unitOfWork.GameRepository.GetAsync(gameId).Result;
            var newGame = new DAL.Models.Game { Id = game.Id, GameStateId = 3 };

            if (!firstCellsWithStateBusyOrHit)
            {
                _unitOfWork.ClearChangeTracker();
                _unitOfWork.GameRepository.Update(newGame);
                _unitOfWork.Commit();

                if (_unitOfWork.GameHistoryRepository.GetAsync(x => x.GameId == gameId).Result == null)
                {
                    _unitOfWork.GameHistoryRepository.Create(new GameHistory
                    {
                        GameId = gameId,
                        FirstPlayerName = _unitOfWork.AppUserRepository.GetAsync(firstPlayerId).Result.UserName,
                        SecondPlayerName = _unitOfWork.AppUserRepository.GetAsync(secondPlayerId).Result.UserName,
                        GameStateName = _unitOfWork.GameStateRepository.GetAsync(newGame.GameStateId).Result.GameStateName,
                        WinnerName = _unitOfWork.AppUserRepository.GetAsync(secondPlayerId).Result.UserName
                    });
                    _unitOfWork.Commit();
                }

                _unitOfWork.ClearChangeTracker();
                _unitOfWork.GameRepository.Update(newGame);
                _unitOfWork.Commit();

                return new IsEndOfTheGameResponse { IsEndOfTheGame = true, WinnerUserName = _unitOfWork.AppUserRepository.GetAsync(secondPlayerId).Result.UserName };
            }

            else if (!secondCellsWithStateBusyOrHit)
            {
                _unitOfWork.ClearChangeTracker();
                _unitOfWork.GameRepository.Update(newGame);
                _unitOfWork.Commit();

                if (_unitOfWork.GameHistoryRepository.GetAsync(x => x.GameId == gameId).Result == null)
                {
                    _unitOfWork.GameHistoryRepository.Create(new GameHistory
                    {
                        GameId = gameId,
                        FirstPlayerName = _unitOfWork.AppUserRepository.GetAsync(firstPlayerId).Result.UserName,
                        SecondPlayerName = _unitOfWork.AppUserRepository.GetAsync(secondPlayerId).Result.UserName,
                        GameStateName = _unitOfWork.GameStateRepository.GetAsync(newGame.GameStateId).Result.GameStateName,
                        WinnerName = _unitOfWork.AppUserRepository.GetAsync(firstPlayerId).Result.UserName
                    });
                    _unitOfWork.Commit();
                }

                _unitOfWork.ClearChangeTracker();
                _unitOfWork.GameRepository.Update(newGame);
                _unitOfWork.Commit();
                return new IsEndOfTheGameResponse { IsEndOfTheGame = true, WinnerUserName = username };
            }
            return new IsEndOfTheGameResponse { IsEndOfTheGame = false, WinnerUserName = "" };
        }

        public ClearingDBResponse ClearingDB(ClearingDBRequest clearingDBRequest)
        {
            var username = _gameServiceHelper.GetUsernameByDecodingJwtToken(clearingDBRequest.Token);
            var firstPlayerId = _unitOfWork.AppUserRepository.GetAsync(x => x.UserName == username).Result.Id;
            var secondPlayerId = _gameServiceHelper.GetSecondPlayerId(firstPlayerId).Result;

            var firstFieldId = _unitOfWork.FieldRepository.GetAsync(x => x.PlayerId == firstPlayerId).Result.Id;
            var firstCellList = _gameServiceHelper.GetCellList(firstFieldId).Result;
            var firstShipWrappers = _unitOfWork.ShipWrapperRepository.GetAllAsync(x => x.FieldId == firstFieldId).Result;
            var firstShips = _gameServiceHelper.GetShipList(firstFieldId).Result;

            var secondFieldId = _unitOfWork.FieldRepository.GetAsync(x => x.PlayerId == secondPlayerId).Result.Id;
            var secondCellList = _gameServiceHelper.GetCellList(secondFieldId).Result;
            var secondShipWrappers = _unitOfWork.ShipWrapperRepository.GetAllAsync(x => x.FieldId == secondFieldId).Result;
            var secondShips = _gameServiceHelper.GetShipList(secondFieldId).Result;

            if (secondCellList.Any())
            {
                //delete all cells
                _unitOfWork.CellRepository.DeleteRange(firstCellList);
                _unitOfWork.Commit();

                //delete all ships
                _unitOfWork.ShipRepository.DeleteRange(firstShips);
                _unitOfWork.Commit();

                //update table AppUser(cleanup column IsHit)
                var firstAppUser = _unitOfWork.AppUserRepository.GetAsync(firstPlayerId).Result;
                var newFirstAppUser = _gameServiceHelper.CreateNewAppUser(firstAppUser, null);
                _unitOfWork.ClearChangeTracker();
                _unitOfWork.AppUserRepository.Update(newFirstAppUser);
                _unitOfWork.Commit();

                var secondAppUser = _unitOfWork.AppUserRepository.GetAsync(secondPlayerId).Result;
                var newSecondAppUser = _gameServiceHelper.CreateNewAppUser(secondAppUser, null);
                _unitOfWork.ClearChangeTracker();
                _unitOfWork.AppUserRepository.Update(newSecondAppUser);
                _unitOfWork.Commit();   

                return new ClearingDBResponse { Message = "The database cleanup was successfull!" };
            }
            else
            {
                //delete all cells
                _unitOfWork.CellRepository.DeleteRange(firstCellList);
                _unitOfWork.Commit();

                //delete all ships
                _unitOfWork.ShipRepository.DeleteRange(firstShips);
                _unitOfWork.Commit();

                //delete all shipWrappers
                _unitOfWork.ShipWrapperRepository.DeleteRange(firstShipWrappers);
                _unitOfWork.Commit();

                //delete all shipWrappers
                _unitOfWork.ShipWrapperRepository.DeleteRange(secondShipWrappers);
                _unitOfWork.Commit();

                //delete game form table Game
                var gameId = _unitOfWork.PlayerGameRepository.GetAsync(x => x.FirstPlayerId == firstPlayerId && x.SecondPlayerId == secondPlayerId).Result.GameId;
                var game = _unitOfWork.GameRepository.GetAsync(gameId).Result;
                _unitOfWork.GameRepository.Delete(game);
                _unitOfWork.Commit();

                //delete field from table Field
                var field = _unitOfWork.FieldRepository.GetAsync(firstFieldId).Result; 
                _unitOfWork.FieldRepository.Delete(field);
                _unitOfWork.Commit();

                //delete field from table Field
                var secondField = _unitOfWork.FieldRepository.GetAsync(secondFieldId).Result;
                _unitOfWork.FieldRepository.Delete(secondField);
                _unitOfWork.Commit();

                return new ClearingDBResponse { Message = "The database cleanup was successfull!" };
            }
        }
    }
}