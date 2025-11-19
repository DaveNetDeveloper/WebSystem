using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface IPerfilService : IService<Perfil, Guid>
    {
        Task<IEnumerable<Perfil>> GetByFiltersAsync(PerfilFilters filters,
                                                     IQueryOptions<Perfil>? queryOptions = null);
    }
}