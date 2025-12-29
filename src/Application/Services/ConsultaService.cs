using Application.DTOs.Filters; 
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using System.Collections;
using static Application.Services.DataQueryService;
using static Utilities.ExporterHelper;

namespace Application.Services
{
    public class ConsultaService : IConsultaService
    {
        private readonly IConsultaRepository _repo;
        private readonly IExcelExporter _excelExporter;
        private readonly IExporter _pdfExporter;


        public ConsultaService(IConsultaRepository repo,
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

        public async Task<IEnumerable<Consulta>> GetAllAsync()
            => await _repo.GetAllAsync();

        public async Task<Consulta?> GetByIdAsync(Guid id)
            => await _repo.GetByIdAsync(id);

        public Task<IEnumerable<Consulta>> GetByFiltersAsync(ConsultaFilters filters,
                                                             IQueryOptions<Consulta>? queryOptions = null)
         => _repo.GetByFiltersAsync(filters, queryOptions);

        public async Task<bool> AddAsync(Consulta consulta)
            => await _repo.AddAsync(consulta);

        public async Task<bool> UpdateAsync(Consulta consulta)
            => await _repo.UpdateAsync(consulta);

        public async Task<bool> Remove(Guid id)
              => await _repo.Remove(id);

        public async Task<byte[]> ExportarAsync(ExportFormat formato)
        {
            Type entityType = typeof(Consulta);
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