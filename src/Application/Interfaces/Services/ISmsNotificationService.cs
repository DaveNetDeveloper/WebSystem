using Application.Common;
using Application.DTOs.Filters;
using Application.DTOs.Responses; 
using Application.Interfaces.Common;
using Domain.Entities;
using static Utilities.ExporterHelper;

namespace Application.Interfaces.Services
{
    public interface ISmsNotificationService : IService<SmsNotification, Guid>
    {
        Task<PagedResult<SmsNotification>> GetByFiltersAsync(SmsNotificationFilters filters,
                                                             IQueryOptions<SmsNotification>? queryOptions = null);

        Guid EnviarSms(SmsDTO sms);
        Task<IEnumerable<string>> ObtenerTiposEnvioSms();
        Task<byte[]> ExportarAsync(ExportFormat formato);
    }
}