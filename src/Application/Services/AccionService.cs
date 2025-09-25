using Application.DTOs.Filters;
using Application.DTOs.Responses;
using Application.Interfaces;
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

namespace Application.Services
{
    public class AccionService : IAccionService
    {
        private readonly IAccionRepository _repo;

        public AccionService(IAccionRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<Accion>> GetAllAsync()
            => _repo.GetAllAsync();
        
        public Task<Accion?> GetByIdAsync(Guid id)
          => _repo.GetByIdAsync(id);

        public Task<IEnumerable<Accion>> GetByFiltersAsync(AccionFilters filters,
                                                           IQueryOptions<Accion>? queryOptions = null)
         => _repo.GetByFiltersAsync(filters, queryOptions);

        public Task<bool> AddAsync(Accion accion)
            => _repo.AddAsync(accion);

        public Task<bool> UpdateAsync(Accion accion)
             => _repo.UpdateAsync(accion);

        public Task<bool> Remove(Guid id)
              => _repo.Remove(id);  
    }
}