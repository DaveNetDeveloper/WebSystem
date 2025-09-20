using Application.Common;
using Application.DTOs.Filters;
using Application.Interfaces.Controllers; 
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{   
    [ApiController]
    [Route("[controller]")]
    public class ActividadesController : BaseController<Actividad>, IController<IActionResult, Actividad, int>
    { 
        private readonly IActividadService _actividadService;

        public ActividadesController(ILogger<ActividadesController> logger, IActividadService actividadService)
        {
            _logger = logger;
            _actividadService = actividadService ?? throw new ArgumentNullException(nameof(actividadService));
        }

        [Authorize]
        [HttpGet("ObtenerActividades")]
        public async Task<IActionResult> GetAllAsync()
        {
            var actividades = await _actividadService.GetAllAsync();
            return (actividades != null && actividades.Any()) ? Ok(actividades) : NoContent();
        }

        [Authorize]
        [HttpGet("FiltrarActividades")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] IFilters<Actividad> filters,
                                                           [FromQuery] int? page,
                                                           [FromQuery] int? pageSize,
                                                           [FromQuery] string? orderBy,
                                                           [FromQuery] bool descending = false) {
            var _filters = filters as ActividadFilters;

            if (_filters is null)
                throw new InvalidOperationException("El filtro recibido no es 'ActividadFilters'.");
            //else
                //_filters = ((ActividadFilters)(IFilters<Actividad>)filters);

            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var filtered = await _actividadService.GetByFiltersAsync(_filters, queryOptions);
            return Ok(filtered);
        }

        //[Authorize]
        //[HttpGet("ObtenerActividad/{id}")]
        //public async Task<IActionResult> GetByIdAsync(int id)
        //{
        //    var actividad = await _actividadService.GetByIdAsync(id);
        //    return actividad != null ? Ok(actividad) : NoContent(); 
        //}

        [Authorize]
        [HttpPost("CrearActividad")]
        public async Task<IActionResult> AddAsync([FromBody] Actividad actividad)
        {
            var nuevaActividad = new Actividad {
                id = -1,
                idEntidad = actividad.idEntidad,
                nombre = actividad.nombre,
                descripcion = actividad.descripcion,
                linkEvento = actividad.linkEvento,
                idTipoActividad = actividad.idTipoActividad,
                ubicación = actividad.ubicación,
                popularidad = actividad.popularidad,
                descripcionCorta = actividad.descripcionCorta,
                fechaInicio = actividad.fechaInicio,
                fechaFin = actividad.fechaFin,
                gratis = actividad.gratis,
                activo = actividad.activo,
                informacioExtra = actividad.informacioExtra,
                linkInstagram = actividad.linkInstagram,
                linkFacebook = actividad.linkFacebook,
                linkYoutube = actividad.linkYoutube
            }; 

            var result = await _actividadService.AddAsync(nuevaActividad);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("Actividad:Crear", "Success"));
                return Ok(result);
            } 
        }

        [Authorize]
        [HttpPut("ActualizarActividad")]
        public async Task<IActionResult> UpdateAsync([FromBody] Actividad actividad)
        {
            var result = await _actividadService.UpdateAsync(actividad); 
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("Actividad:Actualizar", "Success"));
                return Ok(result);
            }
        }

        [Authorize]
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(int id)
        { 
            try {
                var result = await _actividadService.Remove(id);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("Actividad:Eliminar", "Success"));
                    return Ok(result);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error eliminando la actividad, {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("Actividad:Eliminar", "Error"), id });
            }  
        }

        //
        // //Bindings - Table relations
        //
        [Authorize]
        [HttpGet("GetImagenesByActividad/{id}")]
        public async Task<IActionResult> GetImagenesByActividad(int id)
        {
            try {
                var imagenesByActivity = await _actividadService.GetImagenesByActividad(id);

                if (imagenesByActivity == null || !imagenesByActivity.Any()) return NotFound();
                else {
                    //_logger.LogInformation(MessageProvider.GetMessage("Actividad:GetImagenes", "Success"));
                    return Ok(imagenesByActivity);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error obteniendo las imagenes de la actividad {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("Actividad:GetImagenes", "Error"), id });
            }
        }
    }
}