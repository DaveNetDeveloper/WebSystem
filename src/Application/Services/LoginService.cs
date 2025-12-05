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
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _repo;
        private readonly IExcelExporter _excelExporter;
        private readonly IExporter _pdfExporter;

        public LoginService(ILoginRepository repo,
                              IExcelExporter excelExporter,
                              IExporter pdfExporter) {
             
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

        public static class LoginType
        {
            public const string Web = "Web";
            public const string Admin = "Admin";
        }

        public Task<IEnumerable<Login>> GetByFiltersAsync(LoginFilters filters,
                                                          IQueryOptions<Login>? queryOptions = null)
         => _repo.GetByFiltersAsync(filters, queryOptions);

        public Task<Login?> GetByIdAsync(Guid id)
            => _repo.GetByIdAsync(id);

        public Task<IEnumerable<Login>> GetAllAsync()
            => _repo.GetAllAsync(); 

        public Task<bool> AddAsync(Login login)
            => _repo.AddAsync(login);

        public Task<bool> UpdateAsync(Login login)
            => _repo.UpdateAsync(login);

        public Task<bool> Remove(Guid id)
              => _repo.Remove(id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataQueryType"></param>
        /// <returns></returns>
        public async Task<byte[]> ExportarAsync(ExportFormat formato) // TODO por implementar en todas las entidades exportables
        {
            Type entityType = typeof(Login);
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