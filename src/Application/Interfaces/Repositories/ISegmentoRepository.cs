using Domain.Entities;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Common;

namespace Application.Interfaces.Repositories
{
    public interface ISegmentoRepository : IRepository<Segmento, int>
    {
        Task<IEnumerable<Segmento>> GetByFiltersAsync(IFilters<Segmento> filters, 
                                                      IQueryOptions<Segmento>? options = null);
    }
}