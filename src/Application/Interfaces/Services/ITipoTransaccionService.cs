using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Domain.Entities;
using static Utilities.ExporterHelper;

namespace Application.Interfaces.Services
{
    public interface ITipoTransaccionService : IService<TipoTransaccion, Guid>
    {
        Task<IEnumerable<TipoTransaccion>> GetByFiltersAsync(TipoTransaccionFilters filters,
                                                             IQueryOptions<TipoTransaccion>? queryOptions = null);
        Task<byte[]> ExportarAsync(ExportFormat formato);
    }
}