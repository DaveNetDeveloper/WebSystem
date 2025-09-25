using Application.DTOs.Filters;
using Application.DTOs.Responses;
using Application.Interfaces;
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

namespace Application.Services
{
    public class CampanaService : ICampanaService
    {
        private readonly ICampanaRepository _repo;

        public CampanaService(ICampanaRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<Campana>> GetAllAsync()
            => _repo.GetAllAsync();
        
        public Task<Campana?> GetByIdAsync(int id)
          => _repo.GetByIdAsync(id);

        public Task<IEnumerable<Campana>> GetByFiltersAsync(CampanaFilters filters,
                                                            IQueryOptions<Campana>? queryOptions = null)
         => _repo.GetByFiltersAsync(filters, queryOptions);

        public Task<bool> AddAsync(Campana campana)
            => _repo.AddAsync(campana);

        public Task<bool> UpdateAsync(Campana campana)
             => _repo.UpdateAsync(campana);

        public Task<bool> Remove(int id)
              => _repo.Remove(id);

        //public async Task<IEnumerable<ImagenesProductoDTO>> GetImagenesByProducto(int id)
        //{
        //    var dtos = new List<ImagenesProductoDTO>(); 
        //    ImagenesProductoDTO dto = null; 

        //    var imagenesProducto = _repo.GetImagenesByProducto(id); 

        //    foreach (var imagenProducto in imagenesProducto.Result) { 
        //        dto = new ImagenesProductoDTO {
        //            IdProducto = id, // imagenProducto.idproducto
        //            Imagen = imagenProducto.imagen,
        //            Id = imagenProducto.id
        //        };  
        //        dtos.Add(dto);
        //    }
        //    return dtos;
        //}
    }
}