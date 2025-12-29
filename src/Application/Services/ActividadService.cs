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
    public class ActividadService : IActividadService
    {
        private readonly IActividadRepository _repo;
        private readonly IExcelExporter _excelExporter;
        private readonly IExporter _pdfExporter;

        public ActividadService(IActividadRepository repo,
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

        public Task<IEnumerable<Actividad>> GetAllAsync()
            => _repo.GetAllAsync();
        public Task<Actividad?> GetByIdAsync(int id)
            => _repo.GetByIdAsync(id);

        public Task<PagedResult<Actividad>> GetByFiltersAsync(ActividadFilters filters,
                                                              IQueryOptions<Actividad>? queryOptions = null)
            => _repo.GetByFiltersAsync(filters, queryOptions);

        public Task<bool> AddAsync(Actividad actividad)
            => _repo.AddAsync(actividad);

        public Task<bool> UpdateAsync(Actividad actividad)
            => _repo.UpdateAsync(actividad);

        public Task<bool> Remove(int id)
              => _repo.Remove(id);

        public Task<IEnumerable<Actividad>> GetActividadesByTipoActividad(Guid idTipoActividad)
             => _repo.GetActividadesByTipoActividad(idTipoActividad); 

        public Task<IEnumerable<Actividad>> GetActividadesByEntidad(int idEntidad)
             => _repo.GetActividadesByEntidad(idEntidad);  

        public async Task<IEnumerable<ImagenesActividadDTO>> GetImagenesByActividad(int idActividad)
        {
            var dtos = new List<ImagenesActividadDTO>();
            ImagenesActividadDTO dto = null;

            var imagenesActividad = _repo.GetImagenesByActividad(idActividad);

            foreach (var imagenActividad in imagenesActividad.Result) {
                dto = new ImagenesActividadDTO {
                    IdActividad = idActividad, // imagenActividad.idActividad
                    Imagen = imagenActividad.imagen,
                    Id = imagenActividad.id
                };
                dtos.Add(dto);
            }
            return dtos;
        }

        public async Task<byte[]> ExportarAsync(ExportFormat formato) 
        {
            Type entityType = typeof(Actividad);
            IEnumerable queryResult = await GetAllAsync();

            byte[] excelBytes = null;
            switch (formato) {
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