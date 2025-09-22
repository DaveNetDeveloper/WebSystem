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
    public class TipoEnvioCorreosController : BaseController<TipoEnvioCorreo>, IController<IActionResult, TipoEnvioCorreo, Guid>
    {  
        private readonly ITipoEnvioCorreoService _tipoEnvioCorreoService;

        public TipoEnvioCorreosController(ILogger<TipoEnvioCorreosController> logger,
                                          ITipoEnvioCorreoService tipoEnvioCorreoService)
        {
            _logger = logger;
            _tipoEnvioCorreoService = tipoEnvioCorreoService ?? throw new ArgumentNullException(nameof(tipoEnvioCorreoService));
        }

        [Authorize]
        [HttpGet("FiltrarTipoEnvioCorreos")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] IFilters<TipoEnvioCorreo>  filters,
                                                           [FromQuery] int? page,
                                                           [FromQuery] int? pageSize,
                                                           [FromQuery] string? orderBy,
                                                           [FromQuery] bool descending = false)
        {
            var _filters = filters as TipoEnvioCorreoFilters;
            if (_filters is null)
                throw new InvalidOperationException("El filtro recibido no es 'TipoEnvioCorreoFilters'.");

            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var filtered = await _tipoEnvioCorreoService.GetByFiltersAsync(_filters, queryOptions);
            return Ok(filtered);
        }

        //[Authorize]
        [HttpGet("ObtenerTipoEnvioCorreos")]
        public async Task<IActionResult> GetAllAsync()
        {
            try {
                _logger.LogInformation("Obteniendo todos los tipos de envio de correos."); 

                var tiposEnvioCorreo = await _tipoEnvioCorreoService.GetAllAsync();
                return (tiposEnvioCorreo != null && tiposEnvioCorreo.Any()) ? Ok(tiposEnvioCorreo) : NoContent();

            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error obteniendo todos los tipos de envio de correos.");
                throw ex;
            }
        }

        //[Authorize]
        [HttpGet("ObtenerTipoEnvioCorreo/{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Obteniendo un tipo de envio de correo por Id.");

                var tipoEnvioCorreo = await _tipoEnvioCorreoService.GetByIdAsync(id);
                return tipoEnvioCorreo != null ? Ok(tipoEnvioCorreo) : NoContent();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo un tipo de envio de correo por Id.");
                throw ex;
            }
        }

        // [Authorize]
        [HttpPost("CrearTipoEnvioCorreo")]
        public async Task<IActionResult> AddAsync([FromBody] TipoEnvioCorreo tipoEnvioCorreo)
        { 
            var result = await _tipoEnvioCorreoService.AddAsync(tipoEnvioCorreo);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("TipoEnvioCorreo:Crear", "Success"));
                return Ok(result);
            } 
        }

        [Authorize]
        [HttpPut("ActualizarTipoEnvioCorreo")]
        public async Task<IActionResult> UpdateAsync([FromBody] TipoEnvioCorreo tipoEnvioCorreo)
        {
            var result = await _tipoEnvioCorreoService.UpdateAsync(tipoEnvioCorreo);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("TipoEnvioCorreo:Actualizar", "Success"));
                return Ok(result);
            }             
        }

        [Authorize] 
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(Guid id)
        {
            try {
                var result = await _tipoEnvioCorreoService.Remove(id);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("TipoEnvioCorreo:Eliminar", "Success"));
                    return Ok(result);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error eliminando el tipo de envio de correo, {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("TipoEnvioCorreo:Eliminar", "Error"), id });
            } 
        }
    }
}