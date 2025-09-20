using Application.DTOs.Filters; 
using Domain.Entities;
using Application.Interfaces.Common;

namespace Application.Interfaces.Services
{
    public interface IRecompensaService : IService<Recompensa, int>
    {
        Task<IEnumerable<Recompensa>> GetByFiltersAsync(RecompensaFilters filters,
                                                        IQueryOptions<Recompensa>? queryOptions = null);
        Task<IEnumerable<Recompensa>> GetRecompensasByUsuario(int idUsuario);
    }
}