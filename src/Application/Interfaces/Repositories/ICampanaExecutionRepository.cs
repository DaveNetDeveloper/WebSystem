using Domain.Entities;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Common;

namespace Application.Interfaces.Repositories
{
    public interface ICampanaExecutionRepository : IRepository<CampanaExecution, Guid>
    {
        Task<IEnumerable<CampanaExecution>> GetByFiltersAsync(IFilters<CampanaExecution> filters, 
                                                              IQueryOptions<CampanaExecution>? options = null); 
    }
}