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
    public class SmsNotificationService : ISmsNotificationService
    {
        private readonly ISmsNotificationRepository _repo;

        public SmsNotificationService(ISmsNotificationRepository repo) {
            _repo = repo;
        }

        public Task<IEnumerable<SmsNotification>> GetAllAsync()
            => _repo.GetAllAsync();
        public Task<SmsNotification?> GetByIdAsync(Guid id)
            => _repo.GetByIdAsync(id);

        public Task<PagedResult<SmsNotification>> GetByFiltersAsync(SmsNotificationFilters filters,
                                                                    IQueryOptions<SmsNotification>? queryOptions = null)
            => _repo.GetByFiltersAsync(filters, queryOptions);

        public Task<bool> AddAsync(SmsNotification smsNotification)
            => _repo.AddAsync(smsNotification);

        public Task<bool> UpdateAsync(SmsNotification smsNotification)
            => _repo.UpdateAsync(smsNotification);

        public Task<bool> Remove(Guid id)
              => _repo.Remove(id); 
       
    }
}