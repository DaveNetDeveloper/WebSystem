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
    public class CampanaExecutionsController : BaseController<CampanaExecution>,
                                               IController<IActionResult, CampanaExecution, Guid>
    {  
        private readonly ICampanaExecutionService _campanaExecutionService;

        public CampanaExecutionsController(ILogger<CampanaExecutionsController> logger, 
                                           ICampanaExecutionService campanaExecutionService) {
            _logger = logger;
            _campanaExecutionService = campanaExecutionService ?? throw new ArgumentNullException(nameof(campanaExecutionService));
        }

        [Authorize]
        [HttpGet("ObtenerCampanaExecutions")]
        public async Task<IActionResult> GetAllAsync()
        {
            try {
                _logger.LogInformation("Obteniendo todas las ejecuciones de las campañas."); 

                var campanaExecutions = await _campanaExecutionService.GetAllAsync();
                if (campanaExecutions == null || !campanaExecutions.Any()) return NoContent();  

                return Ok(campanaExecutions);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error obteniendo todas las ejecuciones de las campañas.");
                return StatusCode(StatusCodes.Status500InternalServerError, 
                                  new { message = "Se produjo un error al obtener las ejecuciones de las campañas." });
            } 
        }

        [Authorize]
        [HttpGet("FiltrarCampanaExecutions")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] IFilters<CampanaExecution> filters,
                                                           [FromQuery] int? page,
                                                           [FromQuery] int? pageSize,
                                                           [FromQuery] string? orderBy,
                                                           [FromQuery] bool descending = false) { 
            var _filters = filters as CampanaExecutionFilters;

            if (_filters is null)
                throw new InvalidOperationException("El filtro recibido no es 'CampanaExecutionFilters'.");

            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var filtered = await _campanaExecutionService.GetByFiltersAsync(_filters, queryOptions);
            return Ok(filtered);
        }
         
        [Authorize]
        [HttpPost("CrearCampanaExecution")]
        public async Task<IActionResult> AddAsync([FromBody] CampanaExecution campanaExecution)
        {
            try {
                _logger.LogInformation("Creando una nueva ejecución de campaña.");

                var result = await _campanaExecutionService.AddAsync(campanaExecution);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("CampanaExecution:Crear", "Success"));
                    return Ok(result);
                }     
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error creando una nueva ejecución de campaña.");
                
            }
            return NoContent();
        }

        [Authorize]
        [HttpPut("ActualizarCampanaExecution")]
        public async Task<IActionResult> UpdateAsync([FromBody] CampanaExecution campanaExecution)
        {
            try {
                _logger.LogInformation("Actualizando una ejecución de campaña.");

                var result = await _campanaExecutionService.UpdateAsync(campanaExecution);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("CampanaExecution:Actualizar", "Success"));
                    return Ok(result);
                } 
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error actualizando una ejecución de campana.");
               
            }
            return NoContent();
        }

        [Authorize]
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(Guid id)
        {
            try {
                _logger.LogInformation("Eliminando una ejecución de campaña por Id.");

                var result = await _campanaExecutionService.Remove(id);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("CampanaExecution:Eliminar", "Success"));
                    return Ok(result);
                }  
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error eliminando una ejecución de campana por Id.");
              
            }
            return NoContent(); 
        } 
    }
}