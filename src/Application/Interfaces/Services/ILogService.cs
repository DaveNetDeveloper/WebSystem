using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface ILogService : IService<Log, Guid>
    {
        Task<IEnumerable<Log>> GetByFiltersAsync(LogFilters filters,
                                                 IQueryOptions<Log>? queryOptions = null);
    }
}