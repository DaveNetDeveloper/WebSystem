
namespace Application.Interfaces.Repositories
{
    public interface IWriteRepository<TEntity, TKey>
    { 
        Task<bool> AddAsync(TEntity entity);
        Task<bool> UpdateAsync(TEntity entity);
        Task<bool> Remove(TKey id); 
    }
}