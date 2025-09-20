//using Application.DTOs.Filters;
using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface ITipoSegmentoService : IService<TipoSegmento, Guid>
    {
        Task<IEnumerable<TipoSegmento>> GetByFiltersAsync(TipoSegmentoFilters filters,
                                                     IQueryOptions<TipoSegmento>? queryOptions = null);
    }
}