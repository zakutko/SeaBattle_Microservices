using GameHistory.DAL.Data;
using GameHistory.DAL.Interfaces;

namespace GameHistory.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dataContext;

        public UnitOfWork(DataContext dataContext)
        {
            _dataContext = dataContext;
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