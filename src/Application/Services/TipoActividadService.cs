using Application.DTOs.Filters; 
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

namespace Application.Services
{
    public class TipoActividadService : ITipoActividadService
    {
        private readonly ITipoActividadRepository _repo;
        public TipoActividadService(ITipoActividadRepository repo) {
            _repo = repo;
        }

        public Task<IEnumerable<TipoActividad>> GetAllAsync()
            => _repo.GetAllAsync();

        public Task<IEnumerable<TipoActividad>> GetByFiltersAsync(TipoActividadFilters filters,
                                                                  IQueryOptions<TipoActividad>? queryOptions = null)
         => _repo.GetByFiltersAsync(filters, queryOptions);

        public Task<TipoActividad?> GetByIdAsync(Guid id)
            => _repo.GetByIdAsync(id);

        public Task<bool> AddAsync(TipoActividad tipoActividad)
            => _repo.AddAsync(tipoActividad);

        public Task<bool> UpdateAsync(TipoActividad tipoActividad)
             => _repo.UpdateAsync(tipoActividad);

        public Task<bool> Remove(Guid id)
              => _repo.Remove(id);


    }
}