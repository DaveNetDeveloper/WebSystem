using Application.Common;
using Application.DTOs.Filters;
using Application.DTOs.Responses; 
using Application.Interfaces.Common;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface ISmsNotificationService : IService<SmsNotification, Guid>
    {
        Task<PagedResult<SmsNotification>> GetByFiltersAsync(SmsNotificationFilters filters,
                                                             IQueryOptions<SmsNotification>? queryOptions = null);

        Guid EnviarSms(Sms sms);
        Task<IEnumerable<string>> ObtenerTiposEnvioSms();
    }
}