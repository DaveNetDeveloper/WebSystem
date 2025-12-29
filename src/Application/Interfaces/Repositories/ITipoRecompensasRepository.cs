using Application.Interfaces.Common;
using Application.Interfaces.DTOs.Filters;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface ITipoRecompensasRepository : IRepository<TipoRecompensa, Guid>
    {
        Task<IEnumerable<TipoRecompensa>> GetByFiltersAsync(IFilters<TipoRecompensa> filters,
                                                            IQueryOptions<TipoRecompensa>? options = null);
    }
}