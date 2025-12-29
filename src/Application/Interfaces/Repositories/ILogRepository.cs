using Domain.Entities;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Common;

namespace Application.Interfaces.Repositories
{
    public interface ILogRepository : IRepository<Log, Guid>
    {
        Task<IEnumerable<Log>> GetByFiltersAsync(IFilters<Log> filters, 
                                                 IQueryOptions<Log>? options = null);
    }
}