using Application.Common;
using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface ILoginService : IService<Login, Guid>
    {
        Task<IEnumerable<Login>> GetByFiltersAsync(LoginFilters filters,
                                                   IQueryOptions<Login>? queryOptions = null);
    }
}