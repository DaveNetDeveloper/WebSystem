using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface IRolService : IService<Rol, Guid>
    {
        Task<IEnumerable<Rol>> GetByFiltersAsync(RolFilters filters,
                                                     IQueryOptions<Rol>? queryOptions = null);
    }
}