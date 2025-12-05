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
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _repo;
        private readonly IExcelExporter _excelExporter;
        private readonly IExporter _pdfExporter;


        public CategoriaService(ICategoriaRepository repo,
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

        public Task<IEnumerable<Categoria>> GetAllAsync()
            => _repo.GetAllAsync();

        public Task<Categoria?> GetByIdAsync(Guid id)
           => _repo.GetByIdAsync(id);

        public Task<IEnumerable<Categoria>> GetByFiltersAsync(CategoriaFilters filters,
                                                             IQueryOptions<Categoria>? queryOptions = null)
         => _repo.GetByFiltersAsync(filters, queryOptions);
        public Task<bool> AddAsync(Categoria categoria)
            => _repo.AddAsync(categoria);

        public Task<bool> UpdateAsync(Categoria categoria)
             => _repo.UpdateAsync(categoria);

        public Task<bool> Remove(Guid id)
              => _repo.Remove(id);

        public async Task<byte[]> ExportarAsync(ExportFormat formato)
        {
            Type entityType = typeof(Categoria);
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