using Kvota.Models.Products;

namespace Kvota.Interfaces
{
    public interface IRepo<T>
    {
        Task<T> AddAsync(T entity);
        
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);

        Task<T> GetOneAsync(Guid id);
        
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllByIdAsync(Guid id);
        Task Update(T entity);
        Task<int> DeleteAsync(Guid id);

        Task<int> DeleteRangeAsync(IEnumerable<Guid> ids);

        Task<int> SaveAsync(T entity);

        public T Add(T entity);
        int Save(T entity);
        T GetOne(Guid id); 
        Task<IEnumerable<T>> GetSearch(string searchString);


    }
}
