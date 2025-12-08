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
    public class TipoEnvioCorreoService : ITipoEnvioCorreoService
    {
        private readonly ITipoEnvioCorreoRepository _repo;
        private readonly IExcelExporter _excelExporter;
        private readonly IExporter _pdfExporter;
        public TipoEnvioCorreoService(ITipoEnvioCorreoRepository repo,
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

        public Task<IEnumerable<TipoEnvioCorreo>> GetAllAsync()
            => _repo.GetAllAsync();

        public Task<IEnumerable<TipoEnvioCorreo>> GetByFiltersAsync(TipoEnvioCorreoFilters filters,
                                                                    IQueryOptions<TipoEnvioCorreo>? queryOptions = null)
         => _repo.GetByFiltersAsync(filters, queryOptions);

        public Task<TipoEnvioCorreo?> GetByIdAsync(Guid id)
            => _repo.GetByIdAsync(id);

        public Task<bool> AddAsync(TipoEnvioCorreo tipoEnvioCorreo)
            => _repo.AddAsync(tipoEnvioCorreo);

        public Task<bool> UpdateAsync(TipoEnvioCorreo tipoEnvioCorreo)
             => _repo.UpdateAsync(tipoEnvioCorreo);

        public Task<bool> Remove(Guid id)
              => _repo.Remove(id);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataQueryType"></param>
        /// <returns></returns>
        public async Task<byte[]> ExportarAsync(ExportFormat formato) // TODO por implementar en todas las entidades exportables
        {
            Type entityType = typeof(TipoEnvioCorreo);
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