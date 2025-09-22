using Domain.Entities;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Common;

namespace Application.Interfaces.Repositories
{
    public interface ITipoActividadRepository : IRepository<TipoActividad, Guid>
    {
        Task<IEnumerable<TipoActividad  >> GetByFiltersAsync(IFilters<TipoActividad> filters, 
                                                             IQueryOptions<TipoActividad>? options = null);
    }
}