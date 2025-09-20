using Application.DTOs.Filters;
using Application.Interfaces;
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

namespace Application.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _repo;

        public CategoriaService(ICategoriaRepository repo) {
            _repo = repo;
        }

        public Task<IEnumerable<Categoria>> GetAllAsync()
            => _repo.GetAllAsync();

        public Task<Categoria?> GetByIdAsync(Guid id)
           => _repo.GetByIdAsync(id);

        public Task<IEnumerable<Categoria>> GetByFiltersAsync(CategoriaFilters filters,
                                                            IQueryOptions<Categoria>? queryOptions = null)
         => _repo.GetByFiltersAsync(filters, queryOptions);
        public Task<bool> AddAsync(Categoria categoria)
            => _repo.AddAsync(categoria);

        public Task<bool> UpdateAsync(Categoria categoria)
             => _repo.UpdateAsync(categoria);

        public Task<bool> Remove(Guid id)
              => _repo.Remove(id);
    }
}