using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Application.Interfaces.DTOs.Filters;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IFAQRepository : IRepository<FAQ, Guid>
    {
        Task<IEnumerable<FAQ>> GetByFiltersAsync(FAQFilters filters, 
                                                 IQueryOptions<FAQ>? options = null);

    }
}