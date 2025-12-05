using Application.DTOs.Filters;
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
    public class FAQService : IFAQService
    {
        private readonly IExcelExporter _excelExporter;
        private readonly IExporter _pdfExporter; 
        private readonly IFAQRepository _repo;

        public FAQService(IFAQRepository repo,
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

        public Task<IEnumerable<FAQ>> GetAllAsync()
            => _repo.GetAllAsync();

        public Task<IEnumerable<FAQ>> GetByFiltersAsync(FAQFilters filters,
                                                        IQueryOptions<FAQ>? queryOptions = null)
         => _repo.GetByFiltersAsync(filters, queryOptions);

        public Task<FAQ?> GetByIdAsync(Guid id)
           => _repo.GetByIdAsync(id);

        public Task<bool> AddAsync(FAQ faq)
            => _repo.AddAsync(faq);

        public Task<bool> UpdateAsync(FAQ faq)
             => _repo.UpdateAsync(faq);

        public Task<bool> Remove(Guid id)
              => _repo.Remove(id);

        public async Task<byte[]> ExportarAsync(ExportFormat formato)
        {
            Type entityType = typeof(FAQ);
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