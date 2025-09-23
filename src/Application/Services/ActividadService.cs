using Application.Common;
using Application.DTOs.Filters;
using Application.DTOs.Responses;
using Application.Interfaces; 
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

namespace Application.Services
{
    public class ActividadService : IActividadService
    {
        private readonly IActividadRepository _repo;

        public ActividadService(IActividadRepository repo) {
            _repo = repo;
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
    }
}