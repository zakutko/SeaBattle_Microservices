using Game.DAL.Data;
using Game.DAL.Interfaces;
using Game.DAL.Models;

namespace Game.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dataContext;
        private readonly IRepository<Models.Game> _gameRepository;
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

        public UnitOfWork (DataContext dataContext, 
            IRepository<Models.Game> gameRepository,
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
            _dataContext = dataContext;
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

        public IRepository<Models.Game> GameRepository
        {
            get { return _gameRepository; }
        }

        public IRepository<PlayerGame> PlayerGameRepository
        {
            get { return _playerGameRepository; }
        }

        public IRepository<AppUser> AppUserRepository
        {
            get { return _appUserRepository; }
        }

        public IRepository<GameState> GameStateRepository
        {
            get { return _gameStateRepository; }
        }

        public IRepository<Field> FieldRepository
        {
            get { return _fieldRepository; }
        }

        public IRepository<GameField> GameFieldRepository
        {
            get { return _gameFieldRepository; }
        }

        public IRepository<ShipWrapper> ShipWrapperRepository
        {
            get { return _shipWrapperRepository; }
        }

        public IRepository<Ship> ShipRepository
        {
            get { return _shipRepository; }
        }

        public IRepository<Position> PositionRepository
        {
            get { return _positionRepository; }
        }

        public IRepository<Cell> CellRepository
        {
            get { return _cellRepository; }
        }

        public IRepository<Direction> DirectionRepository
        {
            get { return _directionRepository; }
        }

        public IRepository<GameHistory> GameHistoryRepository
        {
            get { return _gameHistoryRepository; }
        }

        public void Commit()
        {
            _dataContext.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _dataContext.SaveChangesAsync();
        }

        public void Rollback()
        {
            _dataContext.Dispose();
        }

        public async Task RollbackAsync()
        {
            await _dataContext.DisposeAsync();
        }

        public void ClearChangeTracker()
        {
            _dataContext.ChangeTracker.Clear();
        }
    }
}