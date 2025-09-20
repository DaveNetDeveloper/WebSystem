using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

namespace Application.Services
{
    public class TipoSegmentoService : ITipoSegmentoService
    {
        private readonly ITipoSegmentoRepository _repo;
        public TipoSegmentoService(ITipoSegmentoRepository repo) {
            _repo = repo;
        }

        public async Task<IEnumerable<TipoSegmento>> GetAllAsync()
            => await _repo.GetAllAsync();

        public async Task<IEnumerable<TipoSegmento>> GetByFiltersAsync(TipoSegmentoFilters filters,
                                                            IQueryOptions<TipoSegmento>? queryOptions = null)
         => await _repo.GetByFiltersAsync(filters, queryOptions);

        public async Task<TipoSegmento?> GetByIdAsync(Guid id)
            => await _repo.GetByIdAsync(id);

        public async Task<bool> AddAsync(TipoSegmento tipoSegmento)
            => await _repo.AddAsync(tipoSegmento);

        public async Task<bool> UpdateAsync(TipoSegmento tipoSegmento)
             => await _repo.UpdateAsync(tipoSegmento);

        public async Task<bool> Remove(Guid id)
              => await _repo.Remove(id);

    }
}