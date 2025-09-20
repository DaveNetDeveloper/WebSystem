using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

namespace Application.Services
{
    public class TipoTransaccionService : ITipoTransaccionService
    {
        private readonly ITipoTransaccionRepository _repo;
        public TipoTransaccionService(ITipoTransaccionRepository repo) {
            _repo = repo;
        }

        public async Task<IEnumerable<TipoTransaccion>> GetAllAsync()
            => await _repo.GetAllAsync();

        public async Task<IEnumerable<TipoTransaccion>> GetByFiltersAsync(TipoTransaccionFilters filters,
                                                                          IQueryOptions<TipoTransaccion>? queryOptions = null)
         => await _repo.GetByFiltersAsync(filters, queryOptions);

        public async Task<TipoTransaccion?> GetByIdAsync(Guid id)
            => await _repo.GetByIdAsync(id);

        public async Task<bool> AddAsync(TipoTransaccion tipoTransaccion)
            => await _repo.AddAsync(tipoTransaccion);

        public async Task<bool> UpdateAsync(TipoTransaccion tipoTransaccion)
             => await _repo.UpdateAsync(tipoTransaccion);

        public async Task<bool> Remove(Guid id)
              => await _repo.Remove(id);

    }
}