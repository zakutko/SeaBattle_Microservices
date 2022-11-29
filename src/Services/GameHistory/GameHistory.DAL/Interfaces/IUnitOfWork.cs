namespace GameHistory.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Models.GameHistory> GameHistoryRepository { get; }
        void Commit();
        void Rollback();
        Task CommitAsync();
        Task RollbackAsync();
        void ClearChangeTracker();
    }
}