using Application.Common;
using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Application.Interfaces.DTOs.Filters;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IInAppNotificationRepository : IRepository<InAppNotification, Guid>
    { 
        Task<PagedResult<InAppNotification>> GetByFiltersAsync(InAppNotificationFilters filters, 
                                                               IQueryOptions<InAppNotification>? options = null,
                                                               CancellationToken cancellationToken = default);
        Task<IEnumerable<string>> ObtenerTiposEnvioInApp();
    }
}