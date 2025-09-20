using Domain.Entities;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Common;

namespace Application.Interfaces.Repositories
{
    public interface ITipoTransaccionRepository : IRepository<TipoTransaccion, Guid>
    {
        Task<IEnumerable<TipoTransaccion>> GetByFiltersAsync(IFilters<TipoTransaccion> filters, 
                                                             IQueryOptions<TipoTransaccion>? options = null);
    }
}