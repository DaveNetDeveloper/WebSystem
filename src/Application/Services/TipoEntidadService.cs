using Application.DTOs.Filters; 
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

namespace Application.Services
{
    public class TipoEntidadService : ITipoEntidadService
    {
        private readonly ITipoEntidadRepository _repo;
        public TipoEntidadService(ITipoEntidadRepository repo) {
            _repo = repo;
        }

        public Task<IEnumerable<TipoEntidad>> GetAllAsync()
            => _repo.GetAllAsync();

        public Task<IEnumerable<TipoEntidad>> GetByFiltersAsync(TipoEntidadFilters filters,
                                                            IQueryOptions<TipoEntidad>? queryOptions = null)
         => _repo.GetByFiltersAsync(filters, queryOptions);

        public Task<TipoEntidad?> GetByIdAsync(Guid id)
            => _repo.GetByIdAsync(id);

        public Task<bool> AddAsync(TipoEntidad transaccion)
            => _repo.AddAsync(transaccion);

        public Task<bool> UpdateAsync(TipoEntidad transaccion)
             => _repo.UpdateAsync(transaccion);

        public Task<bool> Remove(Guid id)
              => _repo.Remove(id);


    }
}