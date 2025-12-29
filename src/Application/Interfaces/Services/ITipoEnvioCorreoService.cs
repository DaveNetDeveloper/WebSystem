using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Domain.Entities;
using static Utilities.ExporterHelper;

namespace Application.Interfaces.Services
{
    public interface ITipoEnvioCorreoService : IService<TipoEnvioCorreo, Guid>
    {
        Task<IEnumerable<TipoEnvioCorreo>> GetByFiltersAsync(TipoEnvioCorreoFilters filters,
                                                             IQueryOptions<TipoEnvioCorreo>? queryOptions = null);
        Task<byte[]> ExportarAsync(ExportFormat formato);
    }
}