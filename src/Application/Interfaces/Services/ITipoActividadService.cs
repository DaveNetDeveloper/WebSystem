using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface ITipoActividadService : IService<TipoActividad, Guid>
    {
        Task<IEnumerable<TipoActividad>> GetByFiltersAsync(TipoActividadFilters filters,
                                                        IQueryOptions<TipoActividad>? queryOptions = null);
    }
}