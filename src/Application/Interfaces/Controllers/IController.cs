using Application.DTOs.Filters;
using Application.Interfaces.DTOs.Filters;
using Domain.Entities;

namespace Application.Interfaces.Controllers
{
    public interface IController<TResponse, TEntity, TId>  
    {
        // TODO: Descomentar y usar para todos los Controllers
        //Task<TResponse> GetByFiltersAsync(IFilters<TEntity> filters, 
        //                                  int? page,
        //                                  int? pageSize,
        //                                  string? orderBy,
        //                                  bool descending = false);
        Task<TResponse> GetAllAsync();
        //Task<TResponse> GetByIdAsync(TId id);
        Task<TResponse> AddAsync(TEntity entity);
        Task<TResponse> UpdateAsync(TEntity entity);
        Task<TResponse> Remove(TId id);
    }
}