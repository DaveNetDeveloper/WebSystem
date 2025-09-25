using Application.DTOs.Filters;
using Application.DTOs.Responses;
using Application.Interfaces;
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

namespace Application.Services
{
    public class CampanaExecutionService : ICampanaExecutionService
    {
        private readonly ICampanaExecutionRepository _repo;

        public CampanaExecutionService(ICampanaExecutionRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<CampanaExecution>> GetAllAsync()
            => _repo.GetAllAsync();
        
        public Task<CampanaExecution?> GetByIdAsync(Guid id)
          => _repo.GetByIdAsync(id);

        public Task<IEnumerable<CampanaExecution>> GetByFiltersAsync(CampanaExecutionFilters filters,
                                                                     IQueryOptions<CampanaExecution>? queryOptions = null)
         => _repo.GetByFiltersAsync(filters, queryOptions);

        public Task<bool> AddAsync(CampanaExecution campanaExecution)
            => _repo.AddAsync(campanaExecution);

        public Task<bool> UpdateAsync(CampanaExecution campanaExecution)
             => _repo.UpdateAsync(campanaExecution);

        public Task<bool> Remove(Guid id)
              => _repo.Remove(id);
    }
}