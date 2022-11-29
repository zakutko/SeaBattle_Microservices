using GameHistory.DAL.Data;
using GameHistory.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GameHistory.DAL.Repositories
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
            return await _dataContext.Set<T>().Where(filter).FirstOrDefaultAsync();
        }

        public async void Create(T entity)
        {
            await _dataContext.Set<T>().AddAsync(entity);
        }

        public async void CreateRange(IEnumerable<T> entities)
        {
            await _dataContext.Set<T>().AddRangeAsync(entities);
        }

        public void Update(T entity)
        {
            _dataContext.Set<T>().Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _dataContext.Set<T>().UpdateRange(entities);
        }

        public void Delete(T entity)
        {
            _dataContext.Set<T>().Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _dataContext.Set<T>().RemoveRange(entities);
        }
    }
}