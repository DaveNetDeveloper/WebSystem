using Application.Common;
using Application.DTOs.Filters;
using Application.Interfaces.Controllers;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Services;
using Application.Services;
using Domain.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccionesController : BaseController<Accion>, IController<IActionResult, Accion, Guid>
    {  
        private readonly IAccionService _accionService;

        public AccionesController(ILogger<AccionesController> logger, IAccionService accionService) {
            _logger = logger;
            _accionService = accionService ?? throw new ArgumentNullException(nameof(accionService));
        }

        //[Authorize]
        [HttpGet("ObtenerAcciones")]
        public async Task<IActionResult> GetAllAsync()
        {
            try {
                _logger.LogInformation("Obteniendo todas las acciones."); 

                var acciones = await _accionService.GetAllAsync();
                if (acciones == null || !acciones.Any()) return NoContent();  

                return Ok(acciones);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error obteniendo todas las acciones.");
                return StatusCode(StatusCodes.Status500InternalServerError, 
                                  new { message = "Se produjo un error al obtener las acciones." });
            } 
        }

        [Authorize]
        [HttpGet("FiltrarAcciones")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] IFilters<Accion> filters,
                                                           [FromQuery] int? page,
                                                           [FromQuery] int? pageSize,
                                                           [FromQuery] string? orderBy,
                                                           [FromQuery] bool descending = false) { 
            var _filters = filters as AccionFilters;

            if (_filters is null)
                throw new InvalidOperationException("El filtro recibido no es 'AccionFilters'.");

            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var filtered = await _accionService.GetByFiltersAsync(_filters, queryOptions);
            return Ok(filtered);
        }

        //[Authorize]
        //[HttpGet("ObtenerAccion/{id}")]
        //public async Task<IActionResult> GetByIdAsync(Guid id)
        //{
        //    try
        //    {
        //        _logger.LogInformation("Obteniendo una acción por Id.");

        //        var accion = await _accionService.GetByIdAsync(id);
        //        if (accion == null) return NoContent();

        //        return Ok(accion);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error obteniendo una acción por Id, {id}.", id);
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //                         new { message = "Se produjo un error al obtener una acción por el Id.", id });
        //    }
        //}

        [Authorize]
        [HttpPost("CrearAccion")]
        public async Task<IActionResult> AddAsync([FromBody] Accion accion)
        {
            try {
                _logger.LogInformation("Creando una nueva acción.");

                var result = await _accionService.AddAsync(accion);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("Accion:Crear", "Success"));
                    return Ok(result);
                }     
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error creando una nueva acción.");
                
            }
            return NoContent();
        }

        [Authorize]
        [HttpPut("ActualizarAccion")]
        public async Task<IActionResult> UpdateAsync([FromBody] Accion accion)
        {
            try {
                _logger.LogInformation("Actualizando una acción.");

                var result = await _accionService.UpdateAsync(accion);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("Accion:Actualizar", "Success"));
                    return Ok(result);
                } 
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error actualizando una acción.");
               
            }
            return NoContent();
        }

        [Authorize]
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(Guid id)
        {
            try {
                _logger.LogInformation("Eliminando una acción por Id.");

                var result = await _accionService.Remove(id);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("Accion:Eliminar", "Success"));
                    return Ok(result);
                }  
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error eliminando una acción por Id.");
              
            }
            return NoContent(); 
        } 
    }
}