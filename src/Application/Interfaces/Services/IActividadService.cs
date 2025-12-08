using Application.Common;
using Application.DTOs.Filters;
using Application.DTOs.Responses; 
using Application.Interfaces.Common;
using Domain.Entities;
using static Utilities.ExporterHelper;

namespace Application.Interfaces.Services
{
    public interface IActividadService : IService<Actividad, int>
    {
        Task<PagedResult<Actividad>> GetByFiltersAsync(ActividadFilters filters,
                                                       IQueryOptions<Actividad>? queryOptions = null);
        Task<IEnumerable<Actividad>> GetActividadesByTipoActividad(Guid idTipoActividad);
        Task<IEnumerable<ImagenesActividadDTO>> GetImagenesByActividad(int idActividad);
        Task<byte[]> ExportarAsync(ExportFormat formato);
    }
}