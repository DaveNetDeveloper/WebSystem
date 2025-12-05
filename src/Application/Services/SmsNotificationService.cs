using Application.Common;
using Application.DTOs.Filters;
using Application.DTOs.Responses;
using Application.Interfaces; 
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.Extensions.Options;
using System.Collections;
using System.Net;
using System.Net.Mail;
using Utilities;
using static Application.Services.DataQueryService;
using static Utilities.ExporterHelper;
namespace Application.Services
{
    public class SmsNotificationService : ISmsNotificationService
    {
        private readonly ISmsNotificationRepository _repo;
        private readonly SmsConfiguration _smsConfig;
        private readonly IExcelExporter _excelExporter;
        private readonly IExporter _pdfExporter;

        public SmsNotificationService(ISmsNotificationRepository repo,
                                      IOptions<SmsConfiguration> smsConfig,
                                      IExcelExporter excelExporter,
                                      IExporter pdfExporter) {
            _repo = repo;
            _smsConfig = smsConfig.Value;
            _excelExporter = excelExporter;
            _pdfExporter = pdfExporter;
        }

        public byte[] ExportDynamic(IEnumerable data, Type entityType)
        {
            return null;
        }

        public byte[] Export<T>(IEnumerable<T> data, string sheetName)
        {
            return null;
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

        public Guid EnviarSms(SmsDTO sms)
        {
            // TODO: Implementar envio de SMS contra envio de mensajeria
            // usando la configuracion desde: [SmsConfiguration]

            var smsSent = new SmsNotification();
            smsSent.id = Guid.NewGuid();


            //Devolvemos el Guid de la nueva notificacion creada en [SmsNotifications]
            return smsSent.id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataQueryType"></param>
        /// <returns></returns>
        public async Task<byte[]> ExportarAsync(ExportFormat formato) // TODO por implementar en todas las entidades exportables
        {
            Type entityType = typeof(SmsNotification);
            IEnumerable queryResult = await GetAllAsync();

            byte[] excelBytes = null;
            switch (formato)
            {
                case ExportFormat.Excel:
                    excelBytes = _excelExporter.ExportToExcelDynamic(queryResult, entityType);
                    break;
                case ExportFormat.Pdf:
                    excelBytes = _pdfExporter.ExportDynamic(queryResult, entityType);
                    break;
            }
            return excelBytes;
        }

        public async Task<byte[]> ExportarAsync(DataQueryType dataQueryType, ExportFormat formato)
        {
            return null;
        }
    }
}