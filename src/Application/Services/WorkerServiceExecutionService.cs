using Application.Common;
using Application.DTOs.Filters;
using Application.DTOs.Responses;
using Application.Interfaces; 
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

namespace Application.Services
{
    public class WorkerServiceExecutionService : IWorkerServiceExecutionService
    {
        private readonly IWorkerServiceExecutionRepository _repo;

        public WorkerServiceExecutionService(IWorkerServiceExecutionRepository repo) {
            _repo = repo;
        }

        public Task<IEnumerable<WorkerServiceExecution>> GetAllAsync()
            => _repo.GetAllAsync();
        public Task<WorkerServiceExecution?> GetByIdAsync(Guid id)
            => _repo.GetByIdAsync(id);

        public Task<bool> AddAsync(WorkerServiceExecution workerServiceExecution)
            => _repo.AddAsync(workerServiceExecution);

        public Task<bool> UpdateAsync(WorkerServiceExecution workerServiceExecution)
            => _repo.UpdateAsync(workerServiceExecution);

        public Task<bool> Remove(Guid id)
              => _repo.Remove(id); 
    }
}