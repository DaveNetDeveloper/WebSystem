using Application.Common;
using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Application.Interfaces.DTOs.Filters;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface ISmsNotificationRepository : IRepository<SmsNotification, Guid>
    { 
        Task<PagedResult<SmsNotification>> GetByFiltersAsync(SmsNotificationFilters filters, 
                                                             IQueryOptions<SmsNotification>? options = null,
                                                             CancellationToken cancellationToken = default);
        Task<IEnumerable<string>> ObtenerTiposEnvioSms();
    }
}