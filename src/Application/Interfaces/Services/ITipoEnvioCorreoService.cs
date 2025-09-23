using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface ITipoEnvioCorreoService : IService<TipoEnvioCorreo, Guid>
    {
        Task<IEnumerable<TipoEnvioCorreo>> GetByFiltersAsync(TipoEnvioCorreoFilters filters,
                                                             IQueryOptions<TipoEnvioCorreo>? queryOptions = null);
    }
}