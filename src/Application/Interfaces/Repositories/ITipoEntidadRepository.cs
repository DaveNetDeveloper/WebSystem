using Domain.Entities;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Common;

namespace Application.Interfaces.Repositories
{
    public interface ITipoEntidadRepository : IRepository<TipoEntidad, Guid>
    {
        Task<IEnumerable<TipoEntidad>> GetByFiltersAsync(IFilters<TipoEntidad> filters, IQueryOptions<TipoEntidad>? options = null);
    }
}