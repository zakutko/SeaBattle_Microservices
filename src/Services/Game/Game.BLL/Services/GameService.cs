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
        private readonly IRepository<DAL.Models.Game> _gameRepository;
        private readonly IRepository<PlayerGame> _playerGameRepository;
        private readonly IRepository<AppUser> _appUserRepository;
        private readonly IRepository<GameState> _gameStateRepository;
        private readonly IRepository<Field> _fieldRepository;
        private readonly IRepository<GameField> _gameFieldRepository;
        private readonly IRepository<ShipWrapper> _shipWrapperRepository;
        private readonly IRepository<Ship> _shipRepository;
        private readonly IRepository<Position> _positionRepository;
        private readonly IRepository<Cell> _cellRepository;
        private readonly IRepository<Direction> _directionRepository;
        private readonly IRepository<GameHistory> _gameHistoryRepository;

        public GameService(
            IUnitOfWork unitOfWork, 
            IGameServiceHelper gameServiceHelper, 
            IMapper mapper,
            IRepository<DAL.Models.Game> gameRepository,
            IRepository<PlayerGame> playerGameRepository,
            IRepository<AppUser> appUserRepository,
            IRepository<GameState> gameStateRepository,
            IRepository<Field> fieldRepository,
            IRepository<GameField> gameFieldRepository,
            IRepository<ShipWrapper> shipWrapperRepository,
            IRepository<Ship> shipRepository,
            IRepository<Position> positionRepository,
            IRepository<Cell> cellRepository,
            IRepository<Direction> directionRepository,
            IRepository<GameHistory> gameHistoryRepository)
        {
            _unitOfWork = unitOfWork;
            _gameServiceHelper = gameServiceHelper;
            _mapper = mapper;
            _gameRepository = gameRepository;
            _playerGameRepository = playerGameRepository;
            _appUserRepository = appUserRepository;
            _gameStateRepository = gameStateRepository;
            _fieldRepository = fieldRepository;
            _gameFieldRepository = gameFieldRepository;
            _shipWrapperRepository = shipWrapperRepository;
            _shipRepository = shipRepository;
            _positionRepository = positionRepository;
            _cellRepository = cellRepository;
            _directionRepository = directionRepository;
            _gameHistoryRepository = gameHistoryRepository;
        }

        public async Task<IEnumerable<GameListResponse>> GetAllGames(GameListRequest gameListRequest)
        {
            var playerGameList = await _playerGameRepository.GetAllAsync();

            var playerGameResponseList = new List<GameListResponse>();

            foreach (var playerGame in playerGameList)
            {
                var game = await _gameRepository.GetAsync(playerGame.GameId);
                var firstPlayer = await _appUserRepository.GetAsync(playerGame.FirstPlayerId);
                var secondPlayer = await _appUserRepository.GetAsync(playerGame.SecondPlayerId);
                var gameState = await _gameStateRepository.GetAsync(game.GameStateId);

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
                    GameState = gameState.GameStateName,
                    NumberOfPlayers = numberOfPlayers
                });
            }

            var username = _gameServiceHelper.GetUsernameByDecodingJwtToken(gameListRequest.Token);

            var playerGameResponseListWithoutCurrUser = playerGameResponseList.Where(playerGame => playerGame.FirstPlayer != username && playerGame.SecondPlayer != username);
            return playerGameResponseListWithoutCurrUser;
        }

        public async Task CreateGame(CreateGameRequest createGameRequest)
        {
            var game = new DAL.Models.Game { GameStateId = 1 };
            await _gameRepository.Create(game);
            _unitOfWork.Commit();
            var gameId = game.Id;

            var username = _gameServiceHelper.GetUsernameByDecodingJwtToken(createGameRequest.Token);
            var appUser = await _appUserRepository.GetAsync(x => x.UserName == username && x.NormalizedUserName == username.ToUpper());
            var newAppUser = _gameServiceHelper.CreateNewAppUser(appUser, true);

            _unitOfWork.ClearChangeTracker();
            _appUserRepository.Update(newAppUser);
            _unitOfWork.Commit();

            var playerGame = new PlayerGame { GameId = gameId, FirstPlayerId = appUser.Id };
            await _playerGameRepository.Create(playerGame);
            _unitOfWork.Commit();

            var field = new Field { Size = 10, PlayerId = appUser.Id };
            await _fieldRepository.Create(field);
            _unitOfWork.Commit();
            var fieldId = field.Id;

            var gameField = new GameField { FirstFieldId = fieldId, GameId = gameId };
            await _gameFieldRepository.Create(gameField);
            _unitOfWork.Commit();

            var numberOfShipsOnField = await _shipWrapperRepository.GetAllAsync(x => x.FieldId == fieldId && x.ShipId != null);
            if (numberOfShipsOnField.Count() == 0)
            {
                var defaultCells = _gameServiceHelper.SetDafaultCells();
                _unitOfWork.ClearChangeTracker();
                _cellRepository.UpdateRange(defaultCells);
                _unitOfWork.Commit();

                var defaultShipWrapper = new ShipWrapper { FieldId = fieldId };
                await _shipWrapperRepository.Create(defaultShipWrapper);
                _unitOfWork.Commit();

                var defaultPositions = defaultCells.Select(cell => new Position { ShipWrapperId = defaultShipWrapper.Id, CellId = cell.Id });
                await _positionRepository.CreateRange(defaultPositions);
                _unitOfWork.Commit();
            }
        }

        public async Task<IsGameOwnerResponse> IsGameOwner(IsGameOwnerRequest isGameOwnerRequest)
        {
            var username = _gameServiceHelper.GetUsernameByDecodingJwtToken(isGameOwnerRequest.Token);
            var player = await _appUserRepository.GetAsync(x => x.UserName == username);

            var playerGame = await _playerGameRepository.GetAsync(x => x.FirstPlayerId == player.Id || x.SecondPlayerId == player.Id);

            if (playerGame == null)
            {
                return new IsGameOwnerResponse
                {
                    IsGameOwner = false,
                    IsSecondPlayerConnected = false
                };
            }

            var isGameOwner = false;
            if (playerGame.FirstPlayerId == player.Id)
            {
                isGameOwner = true;
            }

            var isSecondPlayerConnected = true;
            if (playerGame.FirstPlayerId == player.Id && playerGame.SecondPlayerId == null)
            {
                isSecondPlayerConnected = false;
            }

            return new IsGameOwnerResponse
            {
                IsGameOwner = isGameOwner,
                IsSecondPlayerConnected = isSecondPlayerConnected
            };
        }

        public async Task DeleteGame(DeleteGameRequest deleteGameRequest)
        {
            var username = _gameServiceHelper.GetUsernameByDecodingJwtToken(deleteGameRequest.Token);
            var firstPlayer = await _appUserRepository.GetAsync(x => x.UserName == username);

            var playerGame = await _playerGameRepository.GetAsync(x => x.FirstPlayerId == firstPlayer.Id && x.SecondPlayerId == null);
            var field = await _fieldRepository.GetAsync(x => x.PlayerId == firstPlayer.Id);
            var shipWrappers = await _shipWrapperRepository.GetAllAsync(x => x.FieldId == field.Id);
            var ships = await _gameServiceHelper.GetShipList(field.Id);
            var cellList = await _gameServiceHelper.GetCellList(field.Id);

            if (cellList.Any())
            {
                //delete all shipWrappers
                _shipWrapperRepository.DeleteRange(shipWrappers);
                _unitOfWork.Commit();

                //delete all ships
                _shipRepository.DeleteRange(ships);
                _unitOfWork.Commit();

                //delete all cells
                _cellRepository.DeleteRange(cellList);
                _unitOfWork.Commit();

                //update appUser
                var appUser = await _appUserRepository.GetAsync(firstPlayer.Id);
                var newAppUser = _gameServiceHelper.CreateNewAppUser(appUser, null);
                _unitOfWork.ClearChangeTracker();
                _appUserRepository.Update(newAppUser);
                _unitOfWork.Commit();

                //delete game from table Game
                var deletedGame = await _gameRepository.GetAsync(x => x.Id == playerGame.GameId);
                _gameRepository.Delete(deletedGame);
                _unitOfWork.Commit();

                //delete field from table Field
                var deletedField = await _fieldRepository.GetAsync(x => x.Id == field.Id);
                _fieldRepository.Delete(deletedField);
                _unitOfWork.Commit();
            }
            else
            {
                //delete game from table Game
                _gameRepository.Delete(await _gameRepository.GetAsync(playerGame.Id));
                _unitOfWork.Commit();

                //delete field from table Field
                _fieldRepository.Delete(await _fieldRepository.GetAsync(field.Id));
                _unitOfWork.Commit();

                //update appUser
                var appUser = await _appUserRepository.GetAsync(firstPlayer.Id);
                var newAppUser = _gameServiceHelper.CreateNewAppUser(appUser, null);
                _unitOfWork.ClearChangeTracker();
                _appUserRepository.Update(newAppUser);
                _unitOfWork.Commit();
            }
        }

        public async Task JoinSecondPlayer(JoinSecondPlayerRequest joinSecondPlayerRequest)
        {
            var username = _gameServiceHelper.GetUsernameByDecodingJwtToken(joinSecondPlayerRequest.Token);

            var secondPlayer = await _appUserRepository.GetAsync(x => x.UserName == username);
            var defauldPlayerGame = await _playerGameRepository.GetAsync(x => x.GameId == joinSecondPlayerRequest.GameId);

            var playerGame = _playerGameRepository.GetAsync(x => x.FirstPlayerId == defauldPlayerGame.FirstPlayerId && x.SecondPlayerId == null).Result;

            var defaultGameField = await _gameFieldRepository.GetAsync(x => x.GameId == joinSecondPlayerRequest.GameId);
            var gameField = await _gameFieldRepository.GetAsync(x => x.GameId == joinSecondPlayerRequest.GameId && x.FirstFieldId == defaultGameField.FirstFieldId);

            //update table AppUser
            var appUser = await _appUserRepository.GetAsync(secondPlayer.Id);
            var newAppUser = _gameServiceHelper.CreateNewAppUser(appUser, false);
            _unitOfWork.ClearChangeTracker();
            _appUserRepository.Update(newAppUser);
            _unitOfWork.Commit();

            //update table Game
            var game = await _gameRepository.GetAsync(joinSecondPlayerRequest.GameId);
            var newGame = new DAL.Models.Game
            {
                Id = game.Id,
                GameStateId = 2
            };
            _unitOfWork.ClearChangeTracker();
            _gameRepository.Update(newGame);
            _unitOfWork.Commit();

            //update table Field
            var newField = new Field()
            {
                Size = 10,
                PlayerId = secondPlayer.Id
            };
            _unitOfWork.ClearChangeTracker();
            await _fieldRepository.Create(newField);
            _unitOfWork.Commit();
            var fieldId = newField.Id;

            //update table PlayerGame
            var newPlayerGame = new PlayerGame()
            {
                Id = playerGame.Id,
                GameId = playerGame.GameId,
                FirstPlayerId = playerGame.FirstPlayerId,
                SecondPlayerId = secondPlayer.Id,
                IsReadyFirstPlayer = playerGame.IsReadyFirstPlayer,
                IsReadySecondPlayer = playerGame.IsReadySecondPlayer
            };
            _unitOfWork.ClearChangeTracker();
            _playerGameRepository.Update(newPlayerGame);
            _unitOfWork.Commit();

            //update table GameField
            var newGameField = new GameField()
            {
                Id = gameField.Id,
                FirstFieldId = defaultGameField.FirstFieldId,
                SecondFieldId = newField.Id,
                GameId = joinSecondPlayerRequest.GameId
            };
            _unitOfWork.ClearChangeTracker();
            _gameFieldRepository.Update(newGameField);
            _unitOfWork.Commit();

            var numberOfShipsOnField = await _shipWrapperRepository.GetAllAsync(x => x.FieldId == fieldId && x.ShipId != null);
            if (numberOfShipsOnField.Count() == 0)
            {
                var defaultCells = _gameServiceHelper.SetDafaultCells();
                _unitOfWork.ClearChangeTracker();
                _cellRepository.UpdateRange(defaultCells);
                _unitOfWork.Commit();

                var defaultShipWrapper = new ShipWrapper { FieldId = fieldId };
                await _shipWrapperRepository.Create(defaultShipWrapper);
                _unitOfWork.Commit();

                var defaultPositions = (defaultCells.Select(cell => new Position { ShipWrapperId = defaultShipWrapper.Id, CellId = cell.Id })).ToList();
                await _positionRepository.CreateRange(defaultPositions);
                _unitOfWork.Commit();
            }
        }

        public async Task<IEnumerable<CellListResponse>> GetAllCells(CellListRequest cellListRequest)
        {
            var username = _gameServiceHelper.GetUsernameByDecodingJwtToken(cellListRequest.Token);
            var player = await _appUserRepository.GetAsync(x => x.UserName == username);

            var field = await _fieldRepository.GetAsync(x => x.PlayerId == player.Id);
            var cellList = _gameServiceHelper.GetCellList(field.Id).Result.OrderBy(x => x.Id);
            return cellList.Select(_mapper.Map<CellListResponse>);
        }

        public async Task<CreateShipResponse> CreateShipOnField(CreateShipRequest createShipRequest)
        {
            var username = _gameServiceHelper.GetUsernameByDecodingJwtToken(createShipRequest.Token);
            var player = await _appUserRepository.GetAsync(x => x.UserName == username);

            var field = await _fieldRepository.GetAsync(x => x.PlayerId == player.Id);

            var numberOfShipsOnField = await _shipWrapperRepository.GetAllAsync(x => x.FieldId == field.Id && x.ShipId != null);
            if (numberOfShipsOnField.Count() == 10)
            {
                return new CreateShipResponse { Message = "There are already 10 ships on the field!" };
            }

            var shipWrappers = await _shipWrapperRepository.GetAllAsync(x => x.FieldId == field.Id);

            var ships = shipWrappers.SelectMany(shipWrapperItem => _shipRepository.GetAllAsync(x => x.Id == shipWrapperItem.ShipId).Result);

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
            await _shipRepository.Create(newShip);
            _unitOfWork.Commit();

            //add shipWrapper to ShipWrapper table
            var shipWrapper = new ShipWrapper { ShipId = newShip.Id, FieldId = field.Id };
            await _shipWrapperRepository.Create(shipWrapper);
            _unitOfWork.Commit();

            var direction = await _directionRepository.GetAsync(createShipRequest.ShipDirection);
            try
            {
                var cellListResult = _gameServiceHelper.GetAllCells(direction.DirectionName, createShipRequest.ShipSize, createShipRequest.X, createShipRequest.Y, field.Id).Result;
                if (!cellListResult.Any())
                {
                    _shipWrapperRepository.Delete(shipWrapper);
                    _shipRepository.Delete(newShip);
                    _unitOfWork.Commit();
                }

                var cells = await _gameServiceHelper.GetCellList(field.Id);

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
                        _cellRepository.Update(new Cell { Id = defaultCell.Id, X = cell.X, Y = cell.Y, CellStateId = cell.CellStateId });
                        _unitOfWork.Commit();

                        //update positions in Position table
                        var position = await _positionRepository.GetAsync(x => x.CellId == defaultCell.Id);
                        var updatePosition = new Position
                        {
                            Id = position.Id,
                            ShipWrapperId = shipWrapper.Id,
                            CellId = defaultCell.Id
                        };

                        _unitOfWork.ClearChangeTracker();
                        _positionRepository.Update(updatePosition);
                        _unitOfWork.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                _shipWrapperRepository.Delete(shipWrapper);
                _shipRepository.Delete(newShip);
                _unitOfWork.Commit();
                return new CreateShipResponse { Message = ex.Message };
            }

            return new CreateShipResponse { Message = "Create ship was successful!" };
        }
        
        public async Task<IsPlayerReadyResponse> SetPlayerReady(IsPlayerReadyRequest isPlayerReadyRequest)
        {
            var username = _gameServiceHelper.GetUsernameByDecodingJwtToken(isPlayerReadyRequest.Token);
            var player = await _appUserRepository.GetAsync(x => x.UserName == username);

            var field = await _fieldRepository.GetAsync(x => x.PlayerId == player.Id);
            var shipWrappers = await _shipWrapperRepository.GetAllAsync(x => x.FieldId == field.Id);

            if (shipWrappers.Count() < 11)
            {
                return new IsPlayerReadyResponse { Message = "Number of ships must be 10!" };
            }

            var playerGame = await _playerGameRepository.GetAsync(x => x.FirstPlayerId == player.Id || x.SecondPlayerId == player.Id);
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
                _playerGameRepository.Update(newPlayerGame);
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
                _playerGameRepository.Update(newPlayerGame);
                _unitOfWork.Commit();

                return new IsPlayerReadyResponse { Message = "The Player is ready!" };
            }
        }
        
        public async Task<IsTwoPlayersReadyResponse> IsTwoPlayersReady(IsTwoPlayersReadyRequest isTwoPlayersReadyRequest)
        {
            var username = _gameServiceHelper.GetUsernameByDecodingJwtToken(isTwoPlayersReadyRequest.Token);
            var player = await _appUserRepository.GetAsync(x => x.UserName == username);
            
            var playerGame = await _playerGameRepository.GetAsync(x => x.FirstPlayerId == player.Id || x.SecondPlayerId == player.Id);

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
        
        public async Task<IEnumerable<CellListResponseForSecondPlayer>> GetAllCellForSecondPlayer(CellListRequestForSecondPlayer cellListRequestForSecondPlayer)
        {
            var username = _gameServiceHelper.GetUsernameByDecodingJwtToken(cellListRequestForSecondPlayer.Token);
            var player = await _appUserRepository.GetAsync(x => x.UserName == username);

            var secondPlayerId = await _gameServiceHelper.GetSecondPlayerId(player.Id);

            if (secondPlayerId == null)
            {
                return Enumerable.Empty<CellListResponseForSecondPlayer>();
            }

            var field = await _fieldRepository.GetAsync(x => x.PlayerId == secondPlayerId);
            var cellList = _gameServiceHelper.GetCellList(field.Id).Result.OrderBy(x => x.Id);

            return cellList.Select(_mapper.Map<CellListResponseForSecondPlayer>);
        }
        
        public async Task<ShootResponse> Fire(ShootRequest shootRequest)
        {
            var username = _gameServiceHelper.GetUsernameByDecodingJwtToken(shootRequest.Token);
            var player = await _appUserRepository.GetAsync(x => x.UserName == username);
            var secondPlayerId = await _gameServiceHelper.GetSecondPlayerId(player.Id);

            var field = await _fieldRepository.GetAsync(x => x.PlayerId == secondPlayerId);
            var cellList = await _gameServiceHelper.GetCellList(field.Id);

            var myCell = cellList.Where(x => x.X == shootRequest.X && x.Y == shootRequest.Y).FirstOrDefault();

            if (myCell.CellStateId == 1 || myCell.CellStateId == 5)
            {
                _unitOfWork.ClearChangeTracker();
                _cellRepository.Update(_gameServiceHelper.CreateNewCell(myCell.Id, myCell.X, myCell.Y, myCell.CellStateId, false));
                _unitOfWork.Commit();

                var appUser = await _appUserRepository.GetAsync(x => x.Id == player.Id);
                var newAppUser = _gameServiceHelper.CreateNewAppUser(appUser, false);
                _unitOfWork.ClearChangeTracker();
                _appUserRepository.Update(newAppUser);
                _unitOfWork.Commit();

                var secondAppUser = await _appUserRepository.GetAsync(x => x.Id == secondPlayerId);
                var newSecondAppUser = _gameServiceHelper.CreateNewAppUser(secondAppUser, true);
                _unitOfWork.ClearChangeTracker();
                _appUserRepository.Update(newSecondAppUser);
                _unitOfWork.Commit();

                return new ShootResponse { Message = "Missed the fire!" };
            }
            else
            {
                var position = await _positionRepository.GetAsync(x => x.CellId == myCell.Id);
                var positionsByShipWrapperId = await _positionRepository.GetAllAsync(x => x.ShipWrapperId == position.ShipWrapperId);
                var cellsByCellIds = positionsByShipWrapperId.Select(position => _cellRepository.GetAsync(position.CellId).Result);
                var isDestroyed = cellsByCellIds.Count(x => x.CellStateId == 2) <= 1;

                var newCell = _gameServiceHelper.CreateNewCell(myCell.Id, myCell.X, myCell.Y, myCell.CellStateId, false);

                _unitOfWork.ClearChangeTracker();
                _cellRepository.Update(newCell);
                _unitOfWork.Commit();

                if (isDestroyed)
                {
                    foreach (var cellByCellId in cellsByCellIds)
                    {
                        var newCellByCellId = _gameServiceHelper.CreateNewCell(cellByCellId.Id, cellByCellId.X, cellByCellId.Y, cellByCellId.CellStateId, isDestroyed);

                        _unitOfWork.ClearChangeTracker();
                        _cellRepository.Update(newCellByCellId);
                        _unitOfWork.Commit();
                    }

                    return new ShootResponse { Message = "The ship is destroyed!" };
                }
                return new ShootResponse { Message = "The ship is hit!" };
            }
        }
        
        public async Task<HitResponse> GetPriority(HitRequest hitRequest)
        {
            var username = _gameServiceHelper.GetUsernameByDecodingJwtToken(hitRequest.Token);
            var player = await _appUserRepository.GetAsync(x => x.UserName == username);

            return new HitResponse { IsHit = player.IsHit };
        }

        public async Task<IsEndOfTheGameResponse> IsEndOfTheGame(IsEndOfTheGameRequest isEndOfTheGameRequest)
        {
            var username = _gameServiceHelper.GetUsernameByDecodingJwtToken(isEndOfTheGameRequest.Token);
            var firstPlayer = await _appUserRepository.GetAsync(x => x.UserName == username);
            var firstField = await _fieldRepository.GetAsync(x => x.PlayerId == firstPlayer.Id);
            var firstCellList = await _gameServiceHelper.GetCellList(firstField.Id);

            var playerGame = await _playerGameRepository.GetAsync(x => x.FirstPlayerId == firstPlayer.Id || x.SecondPlayerId == firstPlayer.Id);

            var secondPlayerId = await _gameServiceHelper.GetSecondPlayerId(firstPlayer.Id);
            if (secondPlayerId == null)
            {
                return new IsEndOfTheGameResponse { IsEndOfTheGame = false, WinnerUserName = "" };
            }
            var secondField = await _fieldRepository.GetAsync(x => x.PlayerId == secondPlayerId);
            var secondCellList = await _gameServiceHelper.GetCellList(secondField.Id);

            var gameState = await _gameRepository.GetAsync(playerGame.GameId);
            var firstCellsWithStateBusyOrHit = _gameServiceHelper.CheckIsCellsWithStateBusyOrHit(firstCellList, gameState.GameStateId);
            var secondCellsWithStateBusyOrHit = _gameServiceHelper.CheckIsCellsWithStateBusyOrHit(secondCellList, gameState.GameStateId);

            if (secondCellsWithStateBusyOrHit && firstCellsWithStateBusyOrHit && _gameRepository.GetAsync(playerGame.GameId).Result.GameStateId == 2)
            {
                return new IsEndOfTheGameResponse { IsEndOfTheGame = false, WinnerUserName = "" };
            }

            var game = await _gameRepository.GetAsync(playerGame.GameId);
            var newGame = new DAL.Models.Game { Id = game.Id, GameStateId = 3 };

            if (!firstCellsWithStateBusyOrHit)
            {
                _unitOfWork.ClearChangeTracker();
                _gameRepository.Update(newGame);
                _unitOfWork.Commit();

                if (_gameHistoryRepository.GetAsync(x => x.GameId == game.Id).Result == null)
                {
                    await _gameHistoryRepository.Create(new GameHistory
                    {
                        GameId = game.Id,
                        FirstPlayerName = _appUserRepository.GetAsync(firstPlayer.Id).Result.UserName,
                        SecondPlayerName = _appUserRepository.GetAsync(secondPlayerId).Result.UserName,
                        GameStateName = _gameStateRepository.GetAsync(newGame.GameStateId).Result.GameStateName,
                        WinnerName = _appUserRepository.GetAsync(secondPlayerId).Result.UserName
                    });
                    _unitOfWork.Commit();
                }

                _unitOfWork.ClearChangeTracker();
                _gameRepository.Update(newGame);
                _unitOfWork.Commit();

                return new IsEndOfTheGameResponse { IsEndOfTheGame = true, WinnerUserName = _appUserRepository.GetAsync(secondPlayerId).Result.UserName };
            }

            else if (!secondCellsWithStateBusyOrHit)
            {
                _unitOfWork.ClearChangeTracker();
                _gameRepository.Update(newGame);
                _unitOfWork.Commit();

                if (_gameHistoryRepository.GetAsync(x => x.GameId == game.Id).Result == null)
                {
                    await _gameHistoryRepository.Create(new GameHistory
                    {
                        GameId = game.Id,
                        FirstPlayerName = _appUserRepository.GetAsync(firstPlayer.Id).Result.UserName,
                        SecondPlayerName = _appUserRepository.GetAsync(secondPlayerId).Result.UserName,
                        GameStateName = _gameStateRepository.GetAsync(newGame.GameStateId).Result.GameStateName,
                        WinnerName = _appUserRepository.GetAsync(firstPlayer.Id).Result.UserName
                    });
                    _unitOfWork.Commit();
                }

                _unitOfWork.ClearChangeTracker();
                _gameRepository.Update(newGame);
                _unitOfWork.Commit();
                return new IsEndOfTheGameResponse { IsEndOfTheGame = true, WinnerUserName = username };
            }
            return new IsEndOfTheGameResponse { IsEndOfTheGame = false, WinnerUserName = "" };
        }

        public async Task<ClearingDBResponse> ClearingDB(ClearingDBRequest clearingDBRequest)
        {
            var username = _gameServiceHelper.GetUsernameByDecodingJwtToken(clearingDBRequest.Token);
            var firstPlayer = await _appUserRepository.GetAsync(x => x.UserName == username);
            var secondPlayerId = await _gameServiceHelper.GetSecondPlayerId(firstPlayer.Id);

            var firstField = await _fieldRepository.GetAsync(x => x.PlayerId == firstPlayer.Id);
            var firstCellList = await _gameServiceHelper.GetCellList(firstField.Id);
            var firstShipWrappers = await _shipWrapperRepository.GetAllAsync(x => x.FieldId == firstField.Id);
            var firstShips = await _gameServiceHelper.GetShipList(firstField.Id);

            var secondField = await _fieldRepository.GetAsync(x => x.PlayerId == secondPlayerId);
            var secondCellList = await _gameServiceHelper.GetCellList(secondField.Id);
            var secondShipWrappers = await _shipWrapperRepository.GetAllAsync(x => x.FieldId == secondField.Id);
            var secondShips = await _gameServiceHelper.GetShipList(secondField.Id);

            if (secondCellList.Any())
            {
                //delete all cells
                _cellRepository.DeleteRange(firstCellList);
                _unitOfWork.Commit();

                //delete all ships
                _shipRepository.DeleteRange(firstShips);
                _unitOfWork.Commit();

                //update table AppUser(cleanup column IsHit)
                var firstAppUser = await _appUserRepository.GetAsync(firstPlayer.Id);
                var newFirstAppUser = _gameServiceHelper.CreateNewAppUser(firstAppUser, null);
                _unitOfWork.ClearChangeTracker();
                _appUserRepository.Update(newFirstAppUser);
                _unitOfWork.Commit();

                var secondAppUser = await _appUserRepository.GetAsync(secondPlayerId);
                var newSecondAppUser = _gameServiceHelper.CreateNewAppUser(secondAppUser, null);
                _unitOfWork.ClearChangeTracker();
                _appUserRepository.Update(newSecondAppUser);
                _unitOfWork.Commit();   

                return new ClearingDBResponse { Message = "The database cleanup was successfull!" };
            }
            else
            {
                //delete all cells
                _cellRepository.DeleteRange(firstCellList);
                _unitOfWork.Commit();

                //delete all ships
                _shipRepository.DeleteRange(firstShips);
                _unitOfWork.Commit();

                //delete all shipWrappers
                _shipWrapperRepository.DeleteRange(firstShipWrappers);
                _unitOfWork.Commit();

                //delete all shipWrappers
                _shipWrapperRepository.DeleteRange(secondShipWrappers);
                _unitOfWork.Commit();

                //delete game form table Game
                var playerGame = await _playerGameRepository.GetAsync(x => x.FirstPlayerId == firstPlayer.Id && x.SecondPlayerId == secondPlayerId);
                var game = _gameRepository.GetAsync(playerGame.GameId).Result;
                _gameRepository.Delete(game);
                _unitOfWork.Commit();

                //delete field from table Field
                var field = _fieldRepository.GetAsync(firstField.Id).Result; 
                _fieldRepository.Delete(field);
                _unitOfWork.Commit();

                //delete field from table Field
                var secondFieldDeleted = await _fieldRepository.GetAsync(secondField.Id);
                _fieldRepository.Delete(secondFieldDeleted);
                _unitOfWork.Commit();

                return new ClearingDBResponse { Message = "The database cleanup was successfull!" };
            }
        }
    }
}