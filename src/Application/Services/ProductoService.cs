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
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _repo;
        private readonly IExcelExporter _excelExporter;
        private readonly IExporter _pdfExporter;

        public ProductoService(IProductoRepository repo,
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

        public Task<IEnumerable<Producto>> GetAllAsync()
            => _repo.GetAllAsync();
        
        public Task<Producto?> GetByIdAsync(int id)
          => _repo.GetByIdAsync(id);

        public Task<IEnumerable<Producto>> GetByFiltersAsync(ProductoFilters filters,
                                                            IQueryOptions<Producto>? queryOptions = null)
         => _repo.GetByFiltersAsync(filters, queryOptions);

        public Task<bool> AddAsync(Producto producto)
            => _repo.AddAsync(producto);

        public Task<bool> UpdateAsync(Producto producto)
             => _repo.UpdateAsync(producto);

        public Task<bool> Remove(int id)
              => _repo.Remove(id);

        public async Task<IEnumerable<ImagenesProductoDTO>> GetImagenesByProducto(int id)
        {
            var dtos = new List<ImagenesProductoDTO>(); 
            ImagenesProductoDTO dto = null; 

            var imagenesProducto = _repo.GetImagenesByProducto(id); 

            foreach (var imagenProducto in imagenesProducto.Result) { 
                dto = new ImagenesProductoDTO {
                    IdProducto = id, // imagenProducto.idproducto
                    Imagen = imagenProducto.imagen,
                    Id = imagenProducto.id
                };  
                dtos.Add(dto);
            }
            return dtos;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataQueryType"></param>
        /// <returns></returns>
        public async Task<byte[]> ExportarAsync(ExportFormat formato) // TODO por implementar en todas las entidades exportables
        {
            Type entityType = typeof(Producto);
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