using Domain.Entities;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Common;

namespace Application.Interfaces.Repositories
{
    public interface IFAQRepository : IRepository<FAQ, Guid>
    {
        Task<IEnumerable<FAQ>> GetByFiltersAsync(IFilters<FAQ> filters, IQueryOptions<FAQ>? options = null);

    }
}