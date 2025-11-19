using Application.DTOs.Filters;
using Application.Interfaces;
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

namespace Application.Services
{
    public class PerfilService : IPerfilService
    {
        private readonly IPerfilRepository _repo;

        public PerfilService(IPerfilRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<Perfil>> GetByFiltersAsync(PerfilFilters filters,
                                                           IQueryOptions<Perfil>? queryOptions = null)
         => _repo.GetByFiltersAsync(filters, queryOptions);

        public Task<Perfil?> GetByIdAsync(Guid id)
            => _repo.GetByIdAsync(id);

        public Task<IEnumerable<Perfil>> GetAllAsync()
            => _repo.GetAllAsync(); 

        public Task<bool> AddAsync(Perfil perfil)
            => _repo.AddAsync(perfil);

        public Task<bool> UpdateAsync(Perfil perfil)
            => _repo.UpdateAsync(perfil);

        public Task<bool> Remove(Guid id)
              => _repo.Remove(id); 
    }
}