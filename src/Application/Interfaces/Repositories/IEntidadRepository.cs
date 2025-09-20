using Domain.Entities;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Common;

namespace Application.Interfaces.Repositories
{
    public interface IEntidadRepository : IRepository<Entidad, int>
    {
       Task<IEnumerable<Entidad>> GetByFiltersAsync(IFilters<Entidad> filters, IQueryOptions<Entidad>? options = null);
        Task<IEnumerable<Categoria>> GetCategoriasByEntidad(int id);
    }
}