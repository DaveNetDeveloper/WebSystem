using Application.DTOs.Filters;
using Application.Interfaces;
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using System.Collections;
using static Application.Services.DataQueryService;
using static Utilities.ExporterHelper;
using System.Collections;
using System.Collections;

namespace Application.Services
{
    public class MotivoConsultaService : IMotivoConsultaService
    {
        private readonly IMotivoConsultaRepository _repo;
        private readonly IExcelExporter _excelExporter;
        private readonly IExporter _pdfExporter;

        public MotivoConsultaService(IMotivoConsultaRepository repo,
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

        public async Task<IEnumerable<MotivoConsulta>> GetAllAsync()
            => await _repo.GetAllAsync();

        public async Task<MotivoConsulta?> GetByIdAsync(Guid id)
            => await _repo.GetByIdAsync(id);

        public Task<IEnumerable<MotivoConsulta>> GetByFiltersAsync(MotivoConsultaFilters filters,
                                                            IQueryOptions<MotivoConsulta>? queryOptions = null)
         => _repo.GetByFiltersAsync(filters, queryOptions);

        public async Task<bool> AddAsync(MotivoConsulta motivoConsulta)
            => await _repo.AddAsync(motivoConsulta);

        public async Task<bool> UpdateAsync(MotivoConsulta motivoConsulta)
            => await _repo.UpdateAsync(motivoConsulta);

        public async Task<bool> Remove(Guid id)
              => await _repo.Remove(id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataQueryType"></param>
        /// <returns></returns>
        public async Task<byte[]> ExportarAsync(ExportFormat formato) // TODO por implementar en todas las entidades exportables
        {
            Type entityType = typeof(MotivoConsulta);
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