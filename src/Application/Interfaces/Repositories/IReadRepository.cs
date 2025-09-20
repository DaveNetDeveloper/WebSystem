using Application.Interfaces.Common;
using Application.Interfaces.DTOs.Filters;

namespace Application.Interfaces.Repositories
{
    public interface IReadRepository<TEntity, TKey>
    {
        Task<IEnumerable<TEntity>> GetByFiltersAsync(IFilters<TEntity> filters,
                                                    IQueryOptions<TEntity>? options = null);
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(TKey id); 
    }
}