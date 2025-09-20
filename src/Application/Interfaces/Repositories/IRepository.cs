using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Common;

namespace Application.Interfaces.Repositories
{
    public interface IRepository<TEntity, TKey>
    {
        //Read operations 

        //Task<IEnumerable<TEntity>> GetByFiltersAsync(IFilters<TEntity> filters, IQueryOptions<TEntity>? options = null);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(TKey id);

        // Write operations
        Task<bool> AddAsync(TEntity entity);
        Task<bool> UpdateAsync(TEntity entity);
        Task<bool> Remove(TKey id); 
    }
}