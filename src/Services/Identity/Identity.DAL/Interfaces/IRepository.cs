using System.Linq.Expressions;

namespace Identity.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter);
        Task<T> GetAsync(int? id);
        Task<T> GetAsync(string id);
        Task<T> GetAsync(Expression<Func<T, bool>> filter);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
