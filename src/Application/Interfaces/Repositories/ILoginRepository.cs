using Domain.Entities;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Common;

namespace Application.Interfaces.Repositories
{
    public interface ILoginRepository : IRepository<Login, Guid>
    {
        Task<IEnumerable<Login>> GetByFiltersAsync(IFilters<Login> filters, 
                                                   IQueryOptions<Login>? options = null);
    }
}