using Application.DTOs.Filters;
using Application.Interfaces;
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

namespace Application.Services
{
    public class RolService : IRolService
    {
        private readonly IRolRepository _repo;

        public RolService(IRolRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<Rol>> GetByFiltersAsync(RolFilters filters,
                                                            IQueryOptions<Rol>? queryOptions = null)
         => _repo.GetByFiltersAsync(filters, queryOptions);

        public Task<Rol?> GetByIdAsync(Guid id)
            => _repo.GetByIdAsync(id);

        public Task<IEnumerable<Rol>> GetAllAsync()
            => _repo.GetAllAsync(); 

        public Task<bool> AddAsync(Rol rol)
            => _repo.AddAsync(rol);

        public Task<bool> UpdateAsync(Rol rol)
            => _repo.UpdateAsync(rol);

        public Task<bool> Remove(Guid id)
              => _repo.Remove(id); 
    }
}