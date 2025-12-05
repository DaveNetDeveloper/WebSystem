using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Domain.Entities;
using static Utilities.ExporterHelper;

namespace Application.Interfaces.Services
{
    public interface ITipoActividadService : IService<TipoActividad, Guid>
    {
        Task<IEnumerable<TipoActividad>> GetByFiltersAsync(TipoActividadFilters filters,
                                                        IQueryOptions<TipoActividad>? queryOptions = null);
        Task<byte[]> ExportarAsync(ExportFormat formato);
    }
}