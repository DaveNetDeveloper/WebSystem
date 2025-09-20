using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface ITipoTransaccionService : IService<TipoTransaccion, Guid>
    {
        Task<IEnumerable<TipoTransaccion>> GetByFiltersAsync(TipoTransaccionFilters filters,
                                                             IQueryOptions<TipoTransaccion>? queryOptions = null);
    }
}