using Application.Common;
using Application.DTOs.Filters;
using Application.DTOs.Responses;
using Application.Interfaces; 
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using System.Collections;
using static Application.Services.DataQueryService;
using static Utilities.ExporterHelper;

namespace Application.Services
{
    public class InAppNotificationService : IInAppNotificationService
    {
        private readonly IInAppNotificationRepository _repo;
        private readonly IExcelExporter _excelExporter;
        private readonly IExporter _pdfExporter;
        public InAppNotificationService(IInAppNotificationRepository repo,
                              IExcelExporter excelExporter,
                              IExporter pdfExporter)
        {
            _repo = repo;
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

        public Task<IEnumerable<InAppNotification>> GetAllAsync()
            => _repo.GetAllAsync();
        public Task<InAppNotification?> GetByIdAsync(Guid id)
            => _repo.GetByIdAsync(id);
       
        public Task<PagedResult<InAppNotification>> GetByFiltersAsync(InAppNotificationFilters filters,
                                                                      IQueryOptions<InAppNotification>? queryOptions = null)
            => _repo.GetByFiltersAsync(filters, queryOptions);

        public Task<bool> AddAsync(InAppNotification inAppNotification)
            => _repo.AddAsync(inAppNotification);

        public Task<bool> UpdateAsync(InAppNotification inAppNotification)
            => _repo.UpdateAsync(inAppNotification);

        public Task<bool> Remove(Guid id)
              => _repo.Remove(id);

        //
        //
        //
        public Task<IEnumerable<string>> ObtenerTiposEnvioInApp()
            => _repo.ObtenerTiposEnvioInApp();

        public async Task<byte[]> ExportarAsync(ExportFormat formato)
        {
            Type entityType = typeof(InAppNotification);
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