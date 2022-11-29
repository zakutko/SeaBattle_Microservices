using Identity.DAL.Data;
using Identity.DAL.Interfaces;

namespace Identity.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dataContext;

        public UnitOfWork(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async void Commit()
        {
            await _dataContext.SaveChangesAsync();
        }
    }
}
