using Game.DAL.Models;

namespace Game.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<DAL.Models.Game> GameRepository { get; }
        IRepository<PlayerGame> PlayerGameRepository { get; }
        IRepository<AppUser> AppUserRepository { get; }
        IRepository<GameState> GameStateRepository { get; }
        IRepository<Field> FieldRepository { get; }
        IRepository<GameField> GameFieldRepository { get; }
        IRepository<ShipWrapper> ShipWrapperRepository { get; }
        IRepository<Ship> ShipRepository { get; }
        IRepository<Position> PositionRepository { get; }
        IRepository<Cell> CellRepository { get; }
        IRepository<Direction> DirectionRepository { get; }
        IRepository<GameHistory> GameHistoryRepository { get; }
        void Commit();
        void Rollback();
        Task CommitAsync();
        Task RollbackAsync();
        void ClearChangeTracker();
    }
}
