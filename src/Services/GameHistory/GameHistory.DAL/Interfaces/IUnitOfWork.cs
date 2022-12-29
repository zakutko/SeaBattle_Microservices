namespace GameHistory.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        void Commit();
        void Rollback();
        Task CommitAsync();
        Task RollbackAsync();
        void ClearChangeTracker();
    }
}