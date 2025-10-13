using Application.DTOs.Filters; 
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

namespace Application.Services
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _repo;

        public LogService(ILogRepository repo) {
            _repo = repo;
        }

        public async Task<IEnumerable<Log>> GetAllAsync()
            => await _repo.GetAllAsync();

        public async Task<Log?> GetByIdAsync(Guid id)
            => await _repo.GetByIdAsync(id);

        public Task<IEnumerable<Log>> GetByFiltersAsync(LogFilters filters,
                                                        IQueryOptions<Log>? queryOptions = null)
            => _repo.GetByFiltersAsync(filters, queryOptions);

        public async Task<bool> AddAsync(Log log)
            => await _repo.AddAsync(log);

        public async Task<bool> UpdateAsync(Log log)
            => await _repo.UpdateAsync(log);

        public async Task<bool> Remove(Guid id)
              => await _repo.Remove(id);
    }
}