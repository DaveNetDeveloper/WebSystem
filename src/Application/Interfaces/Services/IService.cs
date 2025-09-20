using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface IService<TEntity, TKey>
    {
        //Task<IEnumerable<TEntity>> GetByFiltersAsync(IFilters<TEntity> filters);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(TKey id); 
        Task<bool> AddAsync(TEntity entidad);
        Task<bool> UpdateAsync(TEntity entidad);
        Task<bool> Remove(TKey id);
    }
}