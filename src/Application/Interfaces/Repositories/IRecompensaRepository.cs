using Domain.Entities;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Common;


namespace Application.Interfaces.Repositories
{
    public interface IRecompensaRepository : IRepository<Recompensa, int>
    {
        Task<IEnumerable<Recompensa>> GetByFiltersAsync(IFilters<Recompensa> filters,
                                                        IQueryOptions<Recompensa>? options = null);
        Task<IEnumerable<Recompensa>> GetRecompensasByUsuario(int idUsuario);
    }
}