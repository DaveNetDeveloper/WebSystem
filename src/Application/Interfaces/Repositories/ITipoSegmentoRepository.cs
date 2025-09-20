using Domain.Entities;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Common;

namespace Application.Interfaces.Repositories
{
    public interface ITipoSegmentoRepository : IRepository<TipoSegmento, Guid>
    {
        Task<IEnumerable<TipoSegmento>> GetByFiltersAsync(IFilters<TipoSegmento> filters, IQueryOptions<TipoSegmento>? options = null);
    }
}