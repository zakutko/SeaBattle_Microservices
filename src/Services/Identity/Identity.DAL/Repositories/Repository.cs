using Identity.DAL.Data;
using Identity.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Identity.DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DataContext _dataContext;

        public Repository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dataContext.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            return await _dataContext.Set<T>().Where(filter).ToListAsync();
        }

        public async Task<T> GetAsync(int? id)
        {
            return await _dataContext.Set<T>().FindAsync(id);
        }

        public async Task<T> GetAsync(string id)
        {
            return await _dataContext.Set<T>().FindAsync(id);
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await _dataContext.Set<T>().FindAsync(filter);
        }

        public void Create(T entity)
        {
            _dataContext.Add(entity);
        }

        public void Update(T entity)
        {
            _dataContext.Update(entity);
        }

        public void Delete(T entity)
        {
            _dataContext.Remove(entity);
        }
    }
}
