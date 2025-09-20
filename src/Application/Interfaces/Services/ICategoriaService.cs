using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Application.Services;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface ICategoriaService : IService<Categoria, Guid>
    {
        public Task<IEnumerable<Categoria>> GetByFiltersAsync(CategoriaFilters filters,
                                                        IQueryOptions<Categoria>? queryOptions = null);
    }
}