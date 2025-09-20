using Application.Common;
using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Application.Interfaces.DTOs.Filters;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IActividadRepository : IRepository<Actividad, int>
    { 
        Task<PagedResult<Actividad>> GetByFiltersAsync(ActividadFilters filters, 
                                                       IQueryOptions<Actividad>? options = null,
                                                       CancellationToken cancellationToken = default);

        Task<IEnumerable<Actividad>> GetActividadesByEntidad(int idEntidad);
        Task<IEnumerable<Actividad>> GetActividadesByTipoEntidad(Guid idTipoEntidad);
        Task<IEnumerable<ActividadImagen>> GetImagenesByActividad(int id);
    }
}