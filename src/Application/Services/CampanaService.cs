using Application.DTOs.Filters;
using Application.DTOs.Responses;
using Application.Interfaces;
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

namespace Application.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class CampanaService : ICampanaService
    {
        private readonly ICampanaRepository _repo;
        private readonly ICampanaSegmentosRepository _repoCampanaSegmentos;
        private readonly ICampanaAccionesRepository _repoCampanaAcciones;
        private readonly IAccionService _accionService;

        /// <summary> Constructor </summary> 
        public CampanaService(ICampanaRepository repo,
                              ICampanaSegmentosRepository repoCampanaSegmentos,
                              ICampanaAccionesRepository repoCampanaAcciones,
                              IAccionService accionService) {
            _repo = repo;
            _repoCampanaSegmentos = repoCampanaSegmentos;
            _repoCampanaAcciones = repoCampanaAcciones;
            _accionService = accionService;
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

        /// <summary>
        /// Obtener los segmentos relacionadas con la campaña
        /// </summary>
        /// <param name="idCampana">Id de la campaña</param>
        /// <returns>Devuelve la lista de segmentos para la campaña</returns>
        public async Task<IEnumerable<Segmento>> GetSegmentoByCampana(int idCampana)
        {
            return await _repoCampanaSegmentos.GetSegmentosByCampana(idCampana); 
        }

        /// <summary>
        /// Obtener las acciones relacionadas con la campaña
        /// </summary>
        /// <param name="idCampana">Id de la campaña</param>
        /// <returns>Devuelve la lista de acciones para la campaña</returns>
        public async Task<IEnumerable<Accion>> GetAccionesByCampana(int idCampana)
        {
            return await _repoCampanaAcciones.GetAccionesByCampana(idCampana); 
        }

        //
        // CRUD para la tabla [CampanaSegmentos]
        //
        public Task<IEnumerable<CampanaSegmentos>> GetCampanaSegmentosAllAsync()
            => _repoCampanaSegmentos.GetCampanaSegmentosAllAsync();

        public CampanaSegmentos GetSegmentosByIdCampanaAsync(CampanaSegmentos campanaSegmentos)
            => _repoCampanaSegmentos.GetSegmentosByIdCampanaAsync(campanaSegmentos);

        public bool AddCampanaSegmentosAsync(CampanaSegmentos campanaSegmentos)
            => _repoCampanaSegmentos.AddCampanaSegmentosAsync(campanaSegmentos);

        public bool UpdateCampanaSegmentosAsync(CampanaSegmentos campanaSegmentos)
             => _repoCampanaSegmentos.UpdateCampanaSegmentosAsync(campanaSegmentos);

        public bool RemoveCampanaSegmentos(CampanaSegmentos campanaSegmentos)
              => _repoCampanaSegmentos.RemoveCampanaSegmentos(campanaSegmentos);

        //
        // CRUD para la tabla [CampanaAcciones]
        //
        public Task<IEnumerable<CampanaAcciones>> GetCampanaAccionesAllAsync()
           => _repoCampanaAcciones.GetCampanaAccionesAllAsync();

        public CampanaAcciones GetAccionesByIdCampanaAsync(CampanaAcciones campanaSegmentos)
            => _repoCampanaAcciones.GetAccionesByIdCampanaAsync(campanaSegmentos);

        public bool AddCampanaAccionAsync(CampanaAcciones campanaSegmentos)
            => _repoCampanaAcciones.AddCampanaAccionAsync(campanaSegmentos);

        public bool UpdateCampanaAccionAsync(CampanaAcciones campanaSegmentos)
             => _repoCampanaAcciones.UpdateCampanaAccionAsync(campanaSegmentos);

        public bool RemoveCampanaAccion(CampanaAcciones campanaSegmentos)
              => _repoCampanaAcciones.RemoveCampanaAccion(campanaSegmentos);
    }
}