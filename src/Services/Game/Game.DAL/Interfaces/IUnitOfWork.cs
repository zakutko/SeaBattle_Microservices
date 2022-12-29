using Game.DAL.Models;

namespace Game.DAL.Interfaces
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
