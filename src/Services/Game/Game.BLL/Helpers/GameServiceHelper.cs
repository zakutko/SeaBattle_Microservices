using Game.BLL.Interfaces;
using Game.DAL.Interfaces;
using Game.DAL.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

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

            return cells.Where(x => x.CellStateId == 2 || x.CellStateId == 3).Count() != 0;
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

        public async Task<IEnumerable<Cell>> GetCellList(int fieldId)
        {
            var shipWrappers = await _unitOfWork.ShipWrapperRepository.GetAllAsync(x => x.FieldId == fieldId);

            var positions = new List<Position>();
            foreach(var shipWrapper in shipWrappers)
            {
                positions.AddRange(await _unitOfWork.PositionRepository.GetAllAsync(x => x.ShipWrapperId == shipWrapper.Id));
            }

            return positions.Select(position => _unitOfWork.CellRepository.GetAsync(position.CellId).Result);
        }

        public async Task<IEnumerable<Ship>> GetShipList(int fieldId)
        {
            var shipWrappers = await _unitOfWork.ShipWrapperRepository.GetAllAsync(x => x.FieldId == fieldId && x.ShipId != null);
            return shipWrappers.Select(shipWrapper => _unitOfWork.ShipRepository.GetAsync(x => x.Id == shipWrapper.ShipId).Result);
        }

        public async Task<string> GetSecondPlayerId(string playerId)
        {
            var playerGame = await _unitOfWork.PlayerGameRepository.GetAsync(x => x.FirstPlayerId == playerId || x.SecondPlayerId == playerId);
            var secondPlayerId = playerGame.FirstPlayerId;
            if (playerGame.FirstPlayerId == playerId)
            {
                secondPlayerId = playerGame.SecondPlayerId;
            }

            return secondPlayerId;
        }

        public async Task<IEnumerable<Cell>> GetAllCells(string shipDirectionName, int shipSize, int X, int Y, int fieldId)
        {
            var cellListResult = new List<Cell>();

            var shipWrapperDefault = await _unitOfWork.ShipWrapperRepository.GetAsync(x => x.FieldId == fieldId && x.ShipId == null);
            var positionsDefault = await _unitOfWork.PositionRepository.GetAllAsync(x => x.ShipWrapperId == shipWrapperDefault.Id);
            var allCellsWithStateNoEmpty = positionsDefault.Select(x => _unitOfWork.CellRepository.GetAsync(x.CellId).Result)
                .Where(x => x.CellStateId == 2 || x.CellStateId == 5);

            switch (shipDirectionName)
            {
                case "Horizontal":
                    var shipCells = new List<Cell>();

                    switch (shipSize)
                    {
                        case 1:
                            shipCells.AddRange(new List<Cell>
                            {
                                new Cell { X = X, Y = Y, CellStateId = 2 },
                                new Cell { X = X, Y = Y - 1, CellStateId = 5},
                                new Cell { X = X + 1, Y = Y, CellStateId = 5},
                                new Cell { X = X, Y = Y + 1, CellStateId = 5},
                                new Cell { X = X - 1, Y = Y, CellStateId = 5},
                                new Cell { X = X - 1, Y = Y - 1, CellStateId = 5},
                                new Cell { X = X + 1, Y = Y - 1, CellStateId = 5},
                                new Cell { X = X + 1, Y = Y + 1, CellStateId = 5},
                                new Cell { X = X - 1, Y = Y + 1, CellStateId = 5},
                            });
                            break;

                        case 2:
                            shipCells.AddRange(new List<Cell>
                            {
                                new Cell { X = X, Y = Y, CellStateId = 2 },
                                new Cell {X = X + 1, Y = Y, CellStateId = 2},

                                new Cell { X = X - 1, Y = Y, CellStateId = 5},
                                new Cell { X = X - 1, Y = Y - 1, CellStateId = 5},
                                new Cell { X = X, Y = Y - 1, CellStateId = 5},
                                new Cell { X = X + 1, Y = Y - 1, CellStateId = 5},
                                new Cell { X = X + 2, Y = Y - 1, CellStateId = 5},
                                new Cell { X = X + 2, Y = Y, CellStateId = 5},
                                new Cell { X = X + 2, Y = Y + 1, CellStateId = 5},
                                new Cell { X = X + 1, Y = Y + 1, CellStateId = 5},
                                new Cell { X = X, Y = Y + 1, CellStateId = 5},
                                new Cell { X = X - 1, Y = Y + 1, CellStateId = 5},
                            });
                            break;

                        case 3:
                            shipCells.AddRange(new List<Cell>
                            {
                                new Cell { X = X, Y = Y, CellStateId = 2 },
                                new Cell {X = X + 1, Y = Y, CellStateId = 2},
                                new Cell {X = X + 2, Y = Y, CellStateId = 2 },

                                new Cell { X = X - 1, Y = Y, CellStateId = 5},
                                new Cell { X = X - 1, Y = Y - 1, CellStateId = 5},
                                new Cell { X = X, Y = Y - 1, CellStateId = 5},
                                new Cell { X = X + 1, Y = Y - 1, CellStateId = 5},
                                new Cell { X = X + 2, Y = Y - 1, CellStateId = 5},
                                new Cell { X = X + 3, Y = Y - 1, CellStateId = 5},
                                new Cell { X = X + 3, Y = Y, CellStateId = 5},
                                new Cell { X = X + 3, Y = Y + 1, CellStateId = 5},
                                new Cell { X = X + 2, Y = Y + 1, CellStateId = 5},
                                new Cell { X = X + 1, Y = Y + 1, CellStateId = 5},
                                new Cell { X = X, Y = Y + 1, CellStateId = 5},
                                new Cell { X = X - 1, Y = Y + 1, CellStateId = 5},
                            });

                            break;

                        case 4:
                            shipCells.AddRange(new List<Cell>
                            {
                                new Cell { X = X, Y = Y, CellStateId = 2 },
                                new Cell {X = X + 1, Y = Y, CellStateId = 2},
                                new Cell {X = X + 2, Y = Y, CellStateId = 2 },
                                new Cell {X = X + 3, Y = Y, CellStateId = 2 },

                                new Cell { X = X - 1, Y = Y, CellStateId = 5},
                                new Cell { X = X - 1, Y = Y - 1, CellStateId = 5},
                                new Cell { X = X, Y = Y - 1, CellStateId = 5},
                                new Cell { X = X + 1, Y = Y - 1, CellStateId = 5},
                                new Cell { X = X + 2, Y = Y - 1, CellStateId = 5},
                                new Cell { X = X + 3, Y = Y - 1, CellStateId = 5},
                                new Cell { X = X + 4, Y = Y - 1, CellStateId = 5},
                                new Cell { X = X + 4, Y = Y, CellStateId = 5},
                                new Cell { X = X + 4, Y = Y + 1, CellStateId = 5},
                                new Cell { X = X + 3, Y = Y + 1, CellStateId = 5},
                                new Cell { X = X + 2, Y = Y + 1, CellStateId = 5},
                                new Cell { X = X + 1, Y = Y + 1, CellStateId = 5},
                                new Cell { X = X, Y = Y + 1, CellStateId = 5},
                                new Cell { X = X - 1, Y = Y + 1, CellStateId = 5},
                            });

                            break;
                    }

                    foreach (var _ in shipCells.Where(shipCell => shipCell.X < 1 || shipCell.X > 10)
                        .Where(shipCell => shipCell.CellStateId == 2)
                        .Select(shipCell => new { }))
                    {
                        throw new Exception("The ship is out of bounds!");
                    }

                    foreach (var _ in allCellsWithStateNoEmpty
                        .SelectMany(cell => shipCells.Where(shipCell => shipCell.CellStateId == 2 && cell.X == shipCell.X && cell.Y == shipCell.Y)
                        .Select(shipCell => new { })))
                    {
                        cellListResult.Clear();
                        break;
                    }

                    cellListResult.AddRange(shipCells);
                    break;

                case "Vertical":
                    shipCells = new List<Cell>();

                    switch (shipSize)
                    {
                        case 1:
                            shipCells.AddRange(new List<Cell>
                            {
                                new Cell { X = X, Y = Y, CellStateId = 2 },

                                new Cell { X = X, Y = Y - 1, CellStateId = 5},
                                new Cell { X = X + 1, Y = Y, CellStateId = 5},
                                new Cell { X = X, Y = Y + 1, CellStateId = 5},
                                new Cell { X = X - 1, Y = Y, CellStateId = 5},
                                new Cell { X = X - 1, Y = Y - 1, CellStateId = 5},
                                new Cell { X = X + 1, Y = Y - 1, CellStateId = 5},
                                new Cell { X = X + 1, Y = Y + 1, CellStateId = 5},
                                new Cell { X = X - 1, Y = Y + 1, CellStateId = 5},
                            });

                            break;

                        case 2:
                            shipCells.AddRange(new List<Cell>
                            {
                                new Cell { X = X, Y = Y, CellStateId = 2 },
                                new Cell { X = X, Y = Y + 1, CellStateId = 2 },

                                new Cell { X = X, Y = Y - 1, CellStateId = 5},
                                new Cell { X = X + 1, Y = Y - 1, CellStateId = 5},
                                new Cell { X = X + 1, Y = Y, CellStateId = 5},
                                new Cell { X = X + 1, Y = Y + 1, CellStateId = 5},
                                new Cell { X = X + 1, Y = Y + 2, CellStateId = 5},
                                new Cell { X = X, Y = Y + 2, CellStateId = 5},
                                new Cell { X = X - 1, Y = Y + 2, CellStateId = 5},
                                new Cell { X = X - 1, Y = Y + 1, CellStateId = 5},
                                new Cell { X = X - 1, Y = Y, CellStateId = 5},
                                new Cell { X = X - 1, Y = Y - 1, CellStateId = 5},
                            });

                            break;

                        case 3:
                            shipCells.AddRange(new List<Cell>
                            {
                                new Cell { X = X, Y = Y, CellStateId = 2 },
                                new Cell { X = X, Y = Y + 1, CellStateId = 2 },
                                new Cell { X = X, Y = Y + 2, CellStateId = 2 },

                                new Cell { X = X, Y = Y - 1, CellStateId = 5},
                                new Cell { X = X + 1, Y = Y - 1, CellStateId = 5},
                                new Cell { X = X + 1, Y = Y, CellStateId = 5},
                                new Cell { X = X + 1, Y = Y + 1, CellStateId = 5},
                                new Cell { X = X + 1, Y = Y + 2, CellStateId = 5},
                                new Cell { X = X + 1, Y = Y + 3, CellStateId = 5},
                                new Cell { X = X, Y = Y + 3, CellStateId = 5},
                                new Cell { X = X - 1, Y = Y + 3, CellStateId = 5},
                                new Cell { X = X - 1, Y = Y + 1, CellStateId = 5},
                                new Cell { X = X - 1, Y = Y + 2, CellStateId = 5},
                                new Cell { X = X - 1, Y = Y, CellStateId = 5},
                                new Cell { X = X - 1, Y = Y - 1, CellStateId = 5},
                            });

                            break;

                        case 4:
                            shipCells.AddRange(new List<Cell>
                            {
                                new Cell { X = X, Y = Y, CellStateId = 2 },
                                new Cell { X = X, Y = Y + 1, CellStateId = 2 },
                                new Cell { X = X, Y = Y + 2, CellStateId = 2 },
                                new Cell { X = X, Y = Y + 3, CellStateId = 2 },

                                new Cell { X = X, Y = Y - 1, CellStateId = 5},
                                new Cell { X = X + 1, Y = Y - 1, CellStateId = 5},
                                new Cell { X = X + 1, Y = Y, CellStateId = 5},
                                new Cell { X = X + 1, Y = Y + 1, CellStateId = 5},
                                new Cell { X = X + 1, Y = Y + 2, CellStateId = 5},
                                new Cell { X = X + 1, Y = Y + 3, CellStateId = 5},
                                new Cell { X = X + 1, Y = Y + 4, CellStateId = 5},
                                new Cell { X = X, Y = Y + 4, CellStateId = 5},
                                new Cell { X = X - 1, Y = Y + 4, CellStateId = 5},
                                new Cell { X = X - 1, Y = Y + 3, CellStateId = 5},
                                new Cell { X = X - 1, Y = Y + 1, CellStateId = 5},
                                new Cell { X = X - 1, Y = Y + 2, CellStateId = 5},
                                new Cell { X = X - 1, Y = Y, CellStateId = 5},
                                new Cell { X = X - 1, Y = Y - 1, CellStateId = 5},
                            });

                            break;
                    }

                    foreach (var _ in shipCells.Where(shipCell => shipCell.Y < 1 || shipCell.Y > 10)
                        .Where(shipCell => shipCell.CellStateId == 2)
                        .Select(shipCell => new { }))
                    {
                        throw new Exception("The ship is out of bounds!");
                    }

                    foreach (var _ in allCellsWithStateNoEmpty
                        .SelectMany(cell => shipCells.Where(shipCell => shipCell.CellStateId == 2 && cell.X == shipCell.X && cell.Y == shipCell.Y)
                        .Select(shipCell => new { })))
                    {
                        cellListResult.Clear();
                        break;
                    }

                    cellListResult.AddRange(shipCells);
                    break;
            }

            return cellListResult.Where(cell => !(cell.X < 1 || cell.X > 10 || cell.Y < 1 || cell.Y > 10));
        }
    }
}