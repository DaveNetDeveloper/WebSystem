using Domain.Entities;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Common;

namespace Application.Interfaces.Repositories
{
    public interface ICategoriaRepository : IRepository<Categoria, Guid>
    {
        Task<IEnumerable<Categoria>> GetByFiltersAsync(IFilters<Categoria> filters, IQueryOptions<Categoria>? options = null);
    }
}