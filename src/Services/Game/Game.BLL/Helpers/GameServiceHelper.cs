using Game.BLL.Interfaces;
using Game.DAL.Interfaces;
using Game.DAL.Models;
using System.IdentityModel.Tokens.Jwt;

namespace Game.BLL.Helpers
{
    public class GameServiceHelper : IGameServiceHelper
    {
        private readonly IUnitOfWork _unitOfWork;

        public GameServiceHelper(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Cell> SetDafaultCells()
        {
            var cells = new List<Cell>();

            for (int i = 1; i <= 10; i++)
            {
                for (int j = 1; j <= 10; j++)
                {
                    cells.Add(new Cell { X = i, Y = j, CellStateId = 1 });
                }
            }
            return cells;
        }

        public Cell CreateNewCell(int id, int x, int y, int cellStateId, bool isDestroyed)
        {
            if (isDestroyed)
            {
                switch (cellStateId)
                {
                    case 2:
                        return new Cell { Id = id, X = x, Y = y, CellStateId = 6 };
                    case 3:
                        return new Cell { Id = id, X = x, Y = y, CellStateId = 6 };
                    default:
                        return new Cell { Id = id, X = x, Y = y, CellStateId = cellStateId };
                }
            }
            switch (cellStateId)
            {
                case 1:
                    return new Cell { Id = id, X = x, Y = y, CellStateId = 4 };
                case 2:
                    return new Cell { Id = id, X = x, Y = y, CellStateId = 3 };
                case 5:
                    return new Cell { Id = id, X = x, Y = y, CellStateId = 4 };
                default:
                    return new Cell { Id = id, X = x, Y = y, CellStateId = cellStateId };
            }
        }

        public bool CheckIsCellsWithStateBusyOrHit(IEnumerable<Cell>? cells, int gameStateId)
        {
            if (cells == null && gameStateId != 3 || cells.Count() == 0 && gameStateId != 3)
            {
                return true;
            }
            var count = cells.Where(x => x.CellStateId == 2 || x.CellStateId == 3).Count();
            if (count == 0)
            {
                return false;
            }
            return true;
        }

        public string GetUsernameByDecodingJwtToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var username = jwtSecurityToken.Claims.First(claim => claim.Type == "unique_name").Value;

            return username;
        }

        public AppUser CreateNewAppUser(AppUser appUser, bool? isHit)
        {
            return new AppUser
            {
                Id = appUser.Id,
                DisplayName = appUser.DisplayName,
                IsHit = isHit,
                UserName = appUser.UserName,
                Email = appUser.Email,
                AccessFailedCount = appUser.AccessFailedCount,
                ConcurrencyStamp = appUser.ConcurrencyStamp,
                SecurityStamp = appUser.ConcurrencyStamp,
                EmailConfirmed = appUser.EmailConfirmed,
                LockoutEnabled = appUser.LockoutEnabled,
                LockoutEnd = appUser.LockoutEnd,
                NormalizedEmail = appUser.NormalizedEmail,
                NormalizedUserName = appUser.NormalizedUserName,
                PasswordHash = appUser.PasswordHash,
                PhoneNumber = appUser.PhoneNumber,
                PhoneNumberConfirmed = appUser.PhoneNumberConfirmed,
                TwoFactorEnabled = appUser.TwoFactorEnabled
            };
        }

        public IEnumerable<Cell> GetCellList(int fieldId)
        {
            var resultList = new List<Cell>();
            var ships = new List<Ship>();
            var positions = new List<Position>();

            var shipWrappers = _unitOfWork.ShipWrapperRepository.GetAllAsync(x => x.FieldId == fieldId).Result;

            foreach (var shipWrapper in shipWrappers)
            {
                positions.AddRange(_unitOfWork.PositionRepository.GetAllAsync(x => x.ShipWrapperId == shipWrapper.Id).Result);
            }

            foreach (var position in positions)
            {
                resultList.Add(_unitOfWork.CellRepository.GetAsync(position.CellId).Result);
            }

            return resultList;
        }

        public IEnumerable<Ship> GetShipList(int fieldId)
        {
            var resultList = new List<Ship>();

            var shipWrappers = _unitOfWork.ShipWrapperRepository.GetAllAsync(x => x.FieldId == fieldId).Result;
            
            foreach (var shipWrapper in shipWrappers)
            {
                resultList.Add(_unitOfWork.ShipRepository.GetAsync(shipWrapper.ShipId).Result);
            }

            return resultList;
        }

        public string GetSecondPlayerId(string playerId)
        {
            var playerGame = _unitOfWork.PlayerGameRepository.GetAsync(x => x.FirstPlayerId == playerId || x.SecondPlayerId == playerId).Result;
            var secondPlayerId = playerGame.FirstPlayerId;
            if (playerGame.FirstPlayerId == playerId)
            {
                secondPlayerId = playerGame.SecondPlayerId;
            }

            return secondPlayerId;
        }

        public IEnumerable<Cell> GetAllCells(string shipDirectionName, int shipSize, int X, int Y, int fieldId)
        {
            var cellListResult = new List<Cell>();
            var shipWrapperDefault = _unitOfWork.ShipWrapperRepository.GetAsync(x => x.FieldId == fieldId && x.ShipId == null).Result;

            var positionsDefault = _unitOfWork.PositionRepository.GetAllAsync(x => x.ShipWrapperId == shipWrapperDefault.Id).Result;

            var cellIds = new List<int>();
            foreach (var position in positionsDefault)
            {
                cellIds.Add(position.CellId);
            }

            var cellList = new List<Cell>();
            foreach (var cellId in cellIds)
            {
                cellList.Add(_unitOfWork.CellRepository.GetAsync(cellId).Result);
            }

            for (int i = 0; i < shipSize; i++)
            {
                if (shipDirectionName == "Vertical")
                {
                    var newCell = new Cell { X = X, Y = Y + i, CellStateId = 2 };
                    if (newCell.Y > 10 || newCell.Y < 1)
                    {
                        throw new Exception("The ship is out of bounds!");
                    }
                    var aroundNewCells = new List<Cell>();
                    if (i == 0)
                    {
                        switch (shipSize)
                        {
                            case 1:
                                aroundNewCells.AddRange(new List<Cell> {
                                        new Cell { X = newCell.X, Y = newCell.Y - 1, CellStateId = 5},
                                        new Cell { X = newCell.X + 1, Y = newCell.Y, CellStateId = 5},
                                        new Cell { X = newCell.X, Y = newCell.Y + 1, CellStateId = 5},
                                        new Cell { X = newCell.X - 1, Y = newCell.Y, CellStateId = 5},
                                        new Cell { X = newCell.X - 1, Y = newCell.Y - 1, CellStateId = 5},
                                        new Cell { X = newCell.X + 1, Y = newCell.Y - 1, CellStateId = 5},
                                        new Cell { X = newCell.X + 1, Y = newCell.Y + 1, CellStateId = 5},
                                        new Cell { X = newCell.X - 1, Y = newCell.Y + 1, CellStateId = 5},
                                    });
                                break;

                            case 2:
                                aroundNewCells.AddRange(new List<Cell> {
                                        new Cell { X = newCell.X, Y = newCell.Y - 1, CellStateId = 5},
                                        new Cell { X = newCell.X + 1, Y = newCell.Y - 1, CellStateId = 5},
                                        new Cell { X = newCell.X + 1, Y = newCell.Y, CellStateId = 5},
                                        new Cell { X = newCell.X + 1, Y = newCell.Y + 1, CellStateId = 5},
                                        new Cell { X = newCell.X + 1, Y = newCell.Y + 2, CellStateId = 5},
                                        new Cell { X = newCell.X, Y = newCell.Y + 2, CellStateId = 5},
                                        new Cell { X = newCell.X - 1, Y = newCell.Y + 2, CellStateId = 5},
                                        new Cell { X = newCell.X - 1, Y = newCell.Y + 1, CellStateId = 5},
                                        new Cell { X = newCell.X - 1, Y = newCell.Y, CellStateId = 5},
                                        new Cell { X = newCell.X - 1, Y = newCell.Y - 1, CellStateId = 5},
                                    });
                                break;
                            case 3:
                                aroundNewCells.AddRange(new List<Cell> {
                                        new Cell { X = newCell.X, Y = newCell.Y - 1, CellStateId = 5},
                                        new Cell { X = newCell.X + 1, Y = newCell.Y - 1, CellStateId = 5},
                                        new Cell { X = newCell.X + 1, Y = newCell.Y, CellStateId = 5},
                                        new Cell { X = newCell.X + 1, Y = newCell.Y + 1, CellStateId = 5},
                                        new Cell { X = newCell.X + 1, Y = newCell.Y + 2, CellStateId = 5},
                                        new Cell { X = newCell.X + 1, Y = newCell.Y + 3, CellStateId = 5},
                                        new Cell { X = newCell.X, Y = newCell.Y + 3, CellStateId = 5},
                                        new Cell { X = newCell.X - 1, Y = newCell.Y + 3, CellStateId = 5},
                                        new Cell { X = newCell.X - 1, Y = newCell.Y + 1, CellStateId = 5},
                                        new Cell { X = newCell.X - 1, Y = newCell.Y + 2, CellStateId = 5},
                                        new Cell { X = newCell.X - 1, Y = newCell.Y, CellStateId = 5},
                                        new Cell { X = newCell.X - 1, Y = newCell.Y - 1, CellStateId = 5},
                                    });
                                break;
                            case 4:
                                aroundNewCells.AddRange(new List<Cell> {
                                        new Cell { X = newCell.X, Y = newCell.Y - 1, CellStateId = 5},
                                        new Cell { X = newCell.X + 1, Y = newCell.Y - 1, CellStateId = 5},
                                        new Cell { X = newCell.X + 1, Y = newCell.Y, CellStateId = 5},
                                        new Cell { X = newCell.X + 1, Y = newCell.Y + 1, CellStateId = 5},
                                        new Cell { X = newCell.X + 1, Y = newCell.Y + 2, CellStateId = 5},
                                        new Cell { X = newCell.X + 1, Y = newCell.Y + 3, CellStateId = 5},
                                        new Cell { X = newCell.X + 1, Y = newCell.Y + 4, CellStateId = 5},
                                        new Cell { X = newCell.X, Y = newCell.Y + 4, CellStateId = 5},
                                        new Cell { X = newCell.X - 1, Y = newCell.Y + 4, CellStateId = 5},
                                        new Cell { X = newCell.X - 1, Y = newCell.Y + 3, CellStateId = 5},
                                        new Cell { X = newCell.X - 1, Y = newCell.Y + 1, CellStateId = 5},
                                        new Cell { X = newCell.X - 1, Y = newCell.Y + 2, CellStateId = 5},
                                        new Cell { X = newCell.X - 1, Y = newCell.Y, CellStateId = 5},
                                        new Cell { X = newCell.X - 1, Y = newCell.Y - 1, CellStateId = 5},
                                    });
                                break;
                        };
                    }
                    var newArroundList = new List<Cell>();
                    foreach (var arroundNewCell in aroundNewCells)
                    {
                        if (!(arroundNewCell.X < 1 || arroundNewCell.X > 10 || arroundNewCell.Y < 1 || arroundNewCell.Y > 10))
                        {
                            newArroundList.Add(arroundNewCell);
                        }
                    }

                    if (!cellList.Any())
                    {
                        cellListResult.Add(newCell);
                        cellListResult.AddRange(newArroundList);
                    }

                    else
                    {
                        var allCellsWithStateNoEmpty = cellList.Where(x => x.CellStateId == 2 || x.CellStateId == 5);
                        var isMatches = false;
                        foreach (var cell in allCellsWithStateNoEmpty)
                        {
                            if (newCell.X == cell.X && newCell.Y == cell.Y)
                            {
                                isMatches = true;
                                break;
                            }
                        }
                        if (isMatches)
                        {
                            aroundNewCells.Clear();
                            cellListResult.Clear();
                        }
                        else
                        {
                            cellListResult.AddRange(newArroundList);
                            cellListResult.Add(newCell);
                        }
                    }
                }
                else if (shipDirectionName == "Horizontal")
                {
                    var newCell = new Cell { X = X + i, Y = Y, CellStateId = 2 };
                    if (newCell.X < 1 || newCell.X > 10)
                    {
                        throw new Exception("The ship is out of bounds!");
                    }

                    var aroundNewCells = new List<Cell>();
                    if (i == 0)
                    {
                        switch (shipSize)
                        {
                            case 1:
                                aroundNewCells.AddRange(new List<Cell> {
                                        new Cell { X = newCell.X, Y = newCell.Y - 1, CellStateId = 5},
                                        new Cell { X = newCell.X + 1, Y = newCell.Y, CellStateId = 5},
                                        new Cell { X = newCell.X, Y = newCell.Y + 1, CellStateId = 5},
                                        new Cell { X = newCell.X - 1, Y = newCell.Y, CellStateId = 5},
                                        new Cell { X = newCell.X - 1, Y = newCell.Y - 1, CellStateId = 5},
                                        new Cell { X = newCell.X + 1, Y = newCell.Y - 1, CellStateId = 5},
                                        new Cell { X = newCell.X + 1, Y = newCell.Y + 1, CellStateId = 5},
                                        new Cell { X = newCell.X - 1, Y = newCell.Y + 1, CellStateId = 5},
                                    });
                                break;
                            case 2:
                                aroundNewCells.AddRange(new List<Cell> {
                                        new Cell { X = newCell.X - 1, Y = newCell.Y, CellStateId = 5},
                                        new Cell { X = newCell.X - 1, Y = newCell.Y - 1, CellStateId = 5},
                                        new Cell { X = newCell.X, Y = newCell.Y - 1, CellStateId = 5},
                                        new Cell { X = newCell.X + 1, Y = newCell.Y - 1, CellStateId = 5},
                                        new Cell { X = newCell.X + 2, Y = newCell.Y - 1, CellStateId = 5},
                                        new Cell { X = newCell.X + 2, Y = newCell.Y, CellStateId = 5},
                                        new Cell { X = newCell.X + 2, Y = newCell.Y + 1, CellStateId = 5},
                                        new Cell { X = newCell.X + 1, Y = newCell.Y + 1, CellStateId = 5},
                                        new Cell { X = newCell.X, Y = newCell.Y + 1, CellStateId = 5},
                                        new Cell { X = newCell.X - 1, Y = newCell.Y + 1, CellStateId = 5},
                                    });
                                break;
                            case 3:
                                aroundNewCells.AddRange(new List<Cell> {
                                        new Cell { X = newCell.X - 1, Y = newCell.Y, CellStateId = 5},
                                        new Cell { X = newCell.X - 1, Y = newCell.Y - 1, CellStateId = 5},
                                        new Cell { X = newCell.X, Y = newCell.Y - 1, CellStateId = 5},
                                        new Cell { X = newCell.X + 1, Y = newCell.Y - 1, CellStateId = 5},
                                        new Cell { X = newCell.X + 2, Y = newCell.Y - 1, CellStateId = 5},
                                        new Cell { X = newCell.X + 3, Y = newCell.Y - 1, CellStateId = 5},
                                        new Cell { X = newCell.X + 3, Y = newCell.Y, CellStateId = 5},
                                        new Cell { X = newCell.X + 3, Y = newCell.Y + 1, CellStateId = 5},
                                        new Cell { X = newCell.X + 2, Y = newCell.Y + 1, CellStateId = 5},
                                        new Cell { X = newCell.X + 1, Y = newCell.Y + 1, CellStateId = 5},
                                        new Cell { X = newCell.X, Y = newCell.Y + 1, CellStateId = 5},
                                        new Cell { X = newCell.X - 1, Y = newCell.Y + 1, CellStateId = 5},
                                    });
                                break;
                            case 4:
                                aroundNewCells.AddRange(new List<Cell> {
                                        new Cell { X = newCell.X - 1, Y = newCell.Y, CellStateId = 5},
                                        new Cell { X = newCell.X - 1, Y = newCell.Y - 1, CellStateId = 5},
                                        new Cell { X = newCell.X, Y = newCell.Y - 1, CellStateId = 5},
                                        new Cell { X = newCell.X + 1, Y = newCell.Y - 1, CellStateId = 5},
                                        new Cell { X = newCell.X + 2, Y = newCell.Y - 1, CellStateId = 5},
                                        new Cell { X = newCell.X + 3, Y = newCell.Y - 1, CellStateId = 5},
                                        new Cell { X = newCell.X + 4, Y = newCell.Y - 1, CellStateId = 5},
                                        new Cell { X = newCell.X + 4, Y = newCell.Y, CellStateId = 5},
                                        new Cell { X = newCell.X + 4, Y = newCell.Y + 1, CellStateId = 5},
                                        new Cell { X = newCell.X + 3, Y = newCell.Y + 1, CellStateId = 5},
                                        new Cell { X = newCell.X + 2, Y = newCell.Y + 1, CellStateId = 5},
                                        new Cell { X = newCell.X + 1, Y = newCell.Y + 1, CellStateId = 5},
                                        new Cell { X = newCell.X, Y = newCell.Y + 1, CellStateId = 5},
                                        new Cell { X = newCell.X - 1, Y = newCell.Y + 1, CellStateId = 5},
                                    });
                                break;
                        };
                    }

                    var newArroundList = new List<Cell>();
                    foreach (var arroundNewCell in aroundNewCells)
                    {
                        if (arroundNewCell.X < 1 || arroundNewCell.X > 10 || arroundNewCell.Y < 1 || arroundNewCell.Y > 10)
                        {
                            continue;
                        }
                        newArroundList.Add(arroundNewCell);
                    }

                    if (!cellList.Any())
                    {
                        cellListResult.Add(newCell);
                        cellListResult.AddRange(newArroundList);
                    }
                    else
                    {
                        var allCellsWithStateNoEmpty = cellList.Where(x => x.CellStateId == 2 || x.CellStateId == 5);
                        var isMatches = false;
                        foreach (var cell in allCellsWithStateNoEmpty)
                        {
                            if (newCell.X == cell.X && newCell.Y == cell.Y)
                            {
                                isMatches = true;
                                break;
                            }
                        }
                        if (isMatches)
                        {
                            aroundNewCells.Clear();
                            cellListResult.Clear();
                        }
                        else
                        {
                            cellListResult.Add(newCell);
                            cellListResult.AddRange(newArroundList);
                        }
                    }
                }
            }

            return cellListResult;
        }
    }
}