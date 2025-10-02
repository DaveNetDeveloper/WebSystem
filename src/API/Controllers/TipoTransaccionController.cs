using Application.Common;
using Application.DTOs.Filters;
using Application.Interfaces.Controllers;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Services;
using Application.Services;
using Domain.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static Domain.Entities.Transaccion;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TipoTransaccionController : BaseController<TipoTransaccion>, IController<IActionResult, TipoTransaccion, Guid>
    { 
        private readonly ITipoTransaccionService _tipoTransaccionService;

        public TipoTransaccionController(ILogger<TipoTransaccionController> logger, ITipoTransaccionService tipoTransaccionService)
        {
            _logger = logger;
            _tipoTransaccionService = tipoTransaccionService ?? throw new ArgumentNullException(nameof(tipoTransaccionService));
        }

        [Authorize]
        [HttpGet("ObtenerTipoTransacciones")]
        public async Task<IActionResult> GetAllAsync()
        {
            var tipoTransacciones = await _tipoTransaccionService.GetAllAsync();
            return (tipoTransacciones != null && tipoTransacciones.Any()) ? Ok(tipoTransacciones) : NoContent();
        }

        [Authorize]
        [HttpGet("FiltrarTipoTransacciones")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] IFilters<TipoTransaccion> filters,
                                                           [FromQuery] int? page,
                                                           [FromQuery] int? pageSize,
                                                           [FromQuery] string? orderBy,
                                                           [FromQuery] bool descending = false) {
            var _filters = filters as TipoTransaccionFilters;

            if (_filters is null)
                throw new InvalidOperationException("El filtro recibido no es 'TipoTransaccionFilters'.");

            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var filtered = await _tipoTransaccionService.GetByFiltersAsync(_filters, queryOptions);
            return Ok(filtered);
        }

        [Authorize]
        [HttpGet("ObtenerTipoTransaccion/{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var tipoTransaccion = await _tipoTransaccionService.GetByIdAsync(id);
            return tipoTransaccion != null ? Ok(tipoTransaccion) : NoContent();
        }

        [Authorize]
        [HttpPost("CrearTipoTransaccion")]
        public async Task<IActionResult> AddAsync([FromBody] TipoTransaccion tipoTransaccion)
        {
            var result = await _tipoTransaccionService.AddAsync(tipoTransaccion);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("TipoTransaccion:Crear", "Success"));
                return Ok(result);
            }   
        }

        [Authorize]
        [HttpPut("ActualizarTipoTransaccion")]
        public async Task<IActionResult> UpdateAsync([FromBody] TipoTransaccion tipoTransaccion)
        {
            var result = await _tipoTransaccionService.UpdateAsync(tipoTransaccion);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("TipoTransaccion:Actualizar", "Success"));
                return Ok(result);
            }  
        }

        [Authorize]
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(Guid id)
        {
            try {
                var result = await _tipoTransaccionService.Remove(id);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("TipoTransaccion:Eliminar", "Success"));
                    return Ok(result);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error eliminando el tipo de transacción, {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("TipoTransaccion:Eliminar", "Error"), id });
            } 
        } 
    } 
}