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
    public class TipoEntidadService : ITipoEntidadService
    {
        private readonly ITipoEntidadRepository _repo;
        private readonly IExcelExporter _excelExporter;
        private readonly IExporter _pdfExporter;
        public TipoEntidadService(ITipoEntidadRepository repo,
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

        public Task<IEnumerable<TipoEntidad>> GetAllAsync()
            => _repo.GetAllAsync();

        public Task<IEnumerable<TipoEntidad>> GetByFiltersAsync(TipoEntidadFilters filters,
                                                            IQueryOptions<TipoEntidad>? queryOptions = null)
         => _repo.GetByFiltersAsync(filters, queryOptions);

        public Task<TipoEntidad?> GetByIdAsync(Guid id)
            => _repo.GetByIdAsync(id);

        public Task<bool> AddAsync(TipoEntidad transaccion)
            => _repo.AddAsync(transaccion);

        public Task<bool> UpdateAsync(TipoEntidad transaccion)
             => _repo.UpdateAsync(transaccion);

        public Task<bool> Remove(Guid id)
              => _repo.Remove(id);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataQueryType"></param>
        /// <returns></returns>
        public async Task<byte[]> ExportarAsync(ExportFormat formato) // TODO por implementar en todas las entidades exportables
        {
            Type entityType = typeof(TipoEntidad);
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