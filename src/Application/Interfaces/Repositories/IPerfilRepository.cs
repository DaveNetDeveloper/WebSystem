using Domain.Entities;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Common;

namespace Application.Interfaces.Repositories
{
    public interface IPerfilRepository : IRepository<Perfil, Guid>
    {
        Task<IEnumerable<Perfil>> GetByFiltersAsync(IFilters<Perfil> filters, 
                                                    IQueryOptions<Perfil>? options = null);
    }
}