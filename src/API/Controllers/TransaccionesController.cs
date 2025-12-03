using Application.Common;
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
    public class TransaccionesController : BaseController<Transaccion>, IController<IActionResult, Transaccion, int>
    {
        private readonly ITransaccionService _transaccionService;

        public TransaccionesController(ILogger<TransaccionesController> logger, ITransaccionService transaccionService)
        {
            _logger = logger; 
            _transaccionService = transaccionService ?? throw new ArgumentNullException(nameof(transaccionService));
        }

        //[AllowAnonymous]
        [Authorize]
        //[EnableRateLimiting("UsuariosLimiter")]
        [HttpGet("FiltrarTransacciones")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] IFilters<Transaccion> filters,
                                                           [FromQuery] int? page,
                                                           [FromQuery] int? pageSize,
                                                           [FromQuery] string? orderBy,
                                                           [FromQuery] bool descending = false) {
            var _filters = filters as TransaccionFilters;
            if (_filters is null)
                throw new InvalidOperationException("El filtro recibido no es 'TransaccionFilters'.");

            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var filteredTransactions = await _transaccionService.GetByFiltersAsync(_filters, queryOptions);
            return Ok(filteredTransactions);
        }

        [Authorize]
        [HttpGet("ObtenerTransacciones")]
        public async Task<IActionResult> GetAllAsync()
        { 
            try {
                _logger.LogInformation("Obteniendo todas las transacciones.");

                var transacciones = await _transaccionService.GetAllAsync();
                return (transacciones != null && transacciones.Any() ? Ok(transacciones) : NoContent());
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error obteniendo todas las transacciones.");
                return NoContent();
            }  
        }
         
        [Authorize]
        [HttpPost("CrearTransaccion")]
        public async Task<IActionResult> AddAsync([FromBody] Transaccion transaccion)
        {
            var result = await _transaccionService.AddAsync(transaccion);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("Transaccion:Crear", "Success"));
                return Ok(result);
            } 
        }

        [Authorize]
        [HttpPut("ActualizarTransaccion")]
        public async Task<IActionResult> UpdateAsync([FromBody] Transaccion transaccion)
        { 
            var result = await _transaccionService.UpdateAsync(transaccion);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("Transaccion:Actualizar", "Success"));
                return Ok(result);
            } 
        }

        [Authorize]
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            try {
                var result = await _transaccionService.Remove(id);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("Transaccion:Eliminar", "Success"));
                    return Ok(result);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error eliminando la transacción, {id}.", id.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("Transaccion:Eliminar", "Error"), id });
            }  
        }
         
    }
}