using Application.DTOs.Filters;
using Application.Interfaces;
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using System.Xml.Linq;

namespace Application.Services
{
    public class EntidadService : IEntidadService
    {
        private readonly IEntidadRepository _repo;
        public EntidadService(IEntidadRepository repo) {
            _repo = repo;
        }

        public Task<IEnumerable<Entidad>> GetAllAsync()
            => _repo.GetAllAsync();

        public Task<IEnumerable<Entidad>> GetByFiltersAsync(EntidadFilters filters,
                                                            IQueryOptions<Entidad>? queryOptions = null)
         => _repo.GetByFiltersAsync(filters, queryOptions);

        public Task<Entidad?> GetByIdAsync(int id)
             => _repo.GetByIdAsync(id);

        public Task<bool> AddAsync(Entidad entidad)
            => _repo.AddAsync(entidad);

        public Task<bool> UpdateAsync(Entidad entidad)
             => _repo.UpdateAsync(entidad);

        public Task<bool> Remove(int id)
              => _repo.Remove(id);

        //
        //Bindings - Table relations
        // 
        public Task<IEnumerable<Categoria>> GetCategoriasByEntidad(int id)
             => _repo.GetCategoriasByEntidad(id);
    }
}