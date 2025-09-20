using Application.DTOs.Filters;
using Application.DTOs.Responses;
using Application.Interfaces;
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

namespace Application.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _repo;

        public ProductoService(IProductoRepository repo)
        {
            _repo = repo;
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
    }
}