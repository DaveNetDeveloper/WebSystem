using Application.DTOs.Filters;
using Application.Interfaces.Controllers;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Services;
using Application.Services;
using Domain.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TipoActividadesController : BaseController<TipoActividad>, IController<IActionResult, TipoActividad, Guid>
    {  
        private readonly ITipoActividadService _tipoActividadService;

        public TipoActividadesController(ILogger<TipoActividadesController> logger, 
                                         ITipoActividadService tipoActividaddService)
        {
            _logger = logger; 
            _tipoActividadService = tipoActividaddService ?? throw new ArgumentNullException(nameof(tipoActividaddService));
        }

        [Authorize]
        [HttpGet("FiltrarTipoActividades")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] IFilters<TipoActividad>  filters,
                                                           [FromQuery] int? page,
                                                           [FromQuery] int? pageSize,
                                                           [FromQuery] string? orderBy,
                                                           [FromQuery] bool descending = false)
        {
            var _filters = filters as TipoActividadFilters;
            if (_filters is null)
                throw new InvalidOperationException("El filtro recibido no es 'TipoActividadFilters'.");

            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var filtered = await _tipoActividadService.GetByFiltersAsync(_filters, queryOptions);
            return Ok(filtered);
        }

        [Authorize]
        [HttpGet("ObtenerTipoActividades")]
        public async Task<IActionResult> GetAllAsync()
        {
            try {
                _logger.LogInformation("Obteniendo todos los tipos de actividad."); 

                var tiposActividad = await _tipoActividadService.GetAllAsync();
                return (tiposActividad != null && tiposActividad.Any()) ? Ok(tiposActividad) : NoContent();

            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error obteniendo todos los tipos de actividad.");
                throw ex;
            }
        }

        [Authorize]
        [HttpGet("ObtenerTipoActividad/{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Obteniendo un tipo de actividad por Id.");

                var tipoActividad = await _tipoActividadService.GetByIdAsync(id);
                return tipoActividad != null ? Ok(tipoActividad) : NoContent();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo un tipo de actividad> por Id.");
                throw ex;
            }
        }

        [Authorize]
        [HttpPost("CrearTipoActividad")]
        public async Task<IActionResult> AddAsync([FromBody] TipoActividad tipoActividad)
        { 
            var result = await _tipoActividadService.AddAsync(tipoActividad);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("TipoActividad:Crear", "Success"));
                return Ok(result);
            } 
        }

        [Authorize]
        [HttpPut("ActualizarTipoActividad")]
        public async Task<IActionResult> UpdateAsync([FromBody] TipoActividad tipoActividad)
        {
            var result = await _tipoActividadService.UpdateAsync(tipoActividad);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("TipoActividad:Actualizar", "Success"));
                return Ok(result);
            }             
        }

        [Authorize] 
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(Guid id)
        {
            try {
                var result = await _tipoActividadService.Remove(id);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("TipoActividad:Eliminar", "Success"));
                    return Ok(result);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error eliminando el tipo de actividad, {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("TipoActividad:Eliminar", "Error"), id });
            } 
        }
    }
}