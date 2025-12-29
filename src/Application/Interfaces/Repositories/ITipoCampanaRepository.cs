using Domain.Entities;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Common;

namespace Application.Interfaces.Repositories
{
    public interface ITipoCampanaRepository : IRepository<TipoCampana, Guid>
    {
        Task<IEnumerable<TipoCampana>> GetByFiltersAsync(IFilters<TipoCampana> filters, 
                                                         IQueryOptions<TipoCampana>? options = null);
    }
}