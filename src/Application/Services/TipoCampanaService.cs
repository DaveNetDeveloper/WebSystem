using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

namespace Application.Services
{
    public class TipoCampanaService : ITipoCampanaService
    {
        private readonly ITipoCampanaRepository _repo;
        public TipoCampanaService(ITipoCampanaRepository repo) {
            _repo = repo;
        }

        public async Task<IEnumerable<TipoCampana>> GetAllAsync()
            => await _repo.GetAllAsync();

        public async Task<IEnumerable<TipoCampana>> GetByFiltersAsync(TipoCampanaFilters filters,
                                                                       IQueryOptions<TipoCampana>? queryOptions = null)
         => await _repo.GetByFiltersAsync(filters, queryOptions);

        public async Task<TipoCampana?> GetByIdAsync(Guid id)
            => await _repo.GetByIdAsync(id);

        public async Task<bool> AddAsync(TipoCampana tipoCampana)
            => await _repo.AddAsync(tipoCampana);

        public async Task<bool> UpdateAsync(TipoCampana tipoCampana)
             => await _repo.UpdateAsync(tipoCampana);

        public async Task<bool> Remove(Guid id)
              => await _repo.Remove(id);
    }
}