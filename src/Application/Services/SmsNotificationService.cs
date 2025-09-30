using Application.Common;
using Application.DTOs.Filters;
using Application.DTOs.Responses;
using Application.Interfaces; 
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using Utilities;

namespace Application.Services
{
    public class SmsNotificationService : ISmsNotificationService
    {
        private readonly ISmsNotificationRepository _repo;
        private readonly SmsConfiguration _smsConfig;

        public SmsNotificationService(ISmsNotificationRepository repo,
                                      IOptions<SmsConfiguration> smsConfig) {
            _repo = repo;
            _smsConfig = smsConfig.Value;
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

        public Task<IEnumerable<string>> ObtenerTiposEnvioSms()
        {
            return _repo.ObtenerTiposEnvioSms();
        }

        public Guid EnviarSms(Sms sms)
        {
            // TODO: Implementar envio de SMS contra envio de mensajeria
            // usando la configuracion desde: [SmsConfiguration]

            var smsSent = new SmsNotification();
            smsSent.id = Guid.NewGuid();


            //Devolvemos el Guid de la nueva notificacion creada en [SmsNotifications]
            return smsSent.id;
        }
    }
}