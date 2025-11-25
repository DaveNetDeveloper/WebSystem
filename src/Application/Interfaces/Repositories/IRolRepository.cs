using Domain.Entities;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Common;

namespace Application.Interfaces.Repositories
{
    public interface IRolRepository : IRepository<Rol, Guid>
    {
        Task<IEnumerable<Rol>> GetByFiltersAsync(IFilters<Rol> filters, IQueryOptions<Rol>? options = null);
 
    }
}