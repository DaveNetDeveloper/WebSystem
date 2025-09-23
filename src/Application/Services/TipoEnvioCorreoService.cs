using Application.DTOs.Filters; 
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

namespace Application.Services
{
    public class TipoEnvioCorreoService : ITipoEnvioCorreoService
    {
        private readonly ITipoEnvioCorreoRepository _repo;
        public TipoEnvioCorreoService(ITipoEnvioCorreoRepository repo) {
            _repo = repo;
        }

        public Task<IEnumerable<TipoEnvioCorreo>> GetAllAsync()
            => _repo.GetAllAsync();

        public Task<IEnumerable<TipoEnvioCorreo>> GetByFiltersAsync(TipoEnvioCorreoFilters filters,
                                                                    IQueryOptions<TipoEnvioCorreo>? queryOptions = null)
         => _repo.GetByFiltersAsync(filters, queryOptions);

        public Task<TipoEnvioCorreo?> GetByIdAsync(Guid id)
            => _repo.GetByIdAsync(id);

        public Task<bool> AddAsync(TipoEnvioCorreo tipoEnvioCorreo)
            => _repo.AddAsync(tipoEnvioCorreo);

        public Task<bool> UpdateAsync(TipoEnvioCorreo tipoEnvioCorreo)
             => _repo.UpdateAsync(tipoEnvioCorreo);

        public Task<bool> Remove(Guid id)
              => _repo.Remove(id);


    }
}