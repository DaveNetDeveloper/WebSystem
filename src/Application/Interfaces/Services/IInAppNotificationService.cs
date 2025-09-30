using Application.Common;
using Application.DTOs.Filters;
using Application.DTOs.Responses; 
using Application.Interfaces.Common;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface IInAppNotificationService : IService<InAppNotification, Guid>
    {
        Task<PagedResult<InAppNotification>> GetByFiltersAsync(InAppNotificationFilters filters,
                                                               IQueryOptions<InAppNotification>? queryOptions = null);
        Task<IEnumerable<string>> ObtenerTiposEnvioInApp();
    }
}