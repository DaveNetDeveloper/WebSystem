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
    public class InAppNotificationService : IInAppNotificationService
    {
        private readonly IInAppNotificationRepository _repo;

        public InAppNotificationService(IInAppNotificationRepository repo) {
            _repo = repo;
        }

        public Task<IEnumerable<InAppNotification>> GetAllAsync()
            => _repo.GetAllAsync();
        public Task<InAppNotification?> GetByIdAsync(Guid id)
            => _repo.GetByIdAsync(id);

        public Task<IEnumerable<string>> ObtenerTiposEnvioInApp()
            => _repo.ObtenerTiposEnvioInApp();
       
        public Task<PagedResult<InAppNotification>> GetByFiltersAsync(InAppNotificationFilters filters,
                                                                      IQueryOptions<InAppNotification>? queryOptions = null)
            => _repo.GetByFiltersAsync(filters, queryOptions);

        public Task<bool> AddAsync(InAppNotification inAppNotification)
            => _repo.AddAsync(inAppNotification);

        public Task<bool> UpdateAsync(InAppNotification inAppNotification)
            => _repo.UpdateAsync(inAppNotification);

        public Task<bool> Remove(Guid id)
              => _repo.Remove(id); 
       
    }
}