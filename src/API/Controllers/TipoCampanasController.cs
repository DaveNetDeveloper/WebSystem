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

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TipoCampanasController : BaseController<TipoCampana>, IController<IActionResult, TipoCampana, Guid>
    { 
        private readonly ITipoCampanaService _tipoCampanaService;

        public TipoCampanasController(ILogger<TipoCampanasController> logger, 
                                      ITipoCampanaService tipoCampanaService)
        {
            _logger = logger;
            _tipoCampanaService = tipoCampanaService ?? throw new ArgumentNullException(nameof(tipoCampanaService));
        }

        [Authorize]
        [HttpGet("ObtenerTipoCampanas")]
        public async Task<IActionResult> GetAllAsync()
        {
            var tipoCampanas = await _tipoCampanaService.GetAllAsync();
            return (tipoCampanas != null && tipoCampanas.Any()) ? Ok(tipoCampanas) : NoContent();
        }

        [Authorize]
        [HttpGet("FiltrarTipoCampanas")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] IFilters<TipoCampana> filters,
                                                           [FromQuery] int? page,
                                                           [FromQuery] int? pageSize,
                                                           [FromQuery] string? orderBy,
                                                           [FromQuery] bool descending = false) {
            var _filters = filters as TipoCampanaFilters;

            if (_filters is null)
                throw new InvalidOperationException("El filtro recibido no es 'TipoCampanaFilters'.");

            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var filtered = await _tipoCampanaService.GetByFiltersAsync(_filters, queryOptions);
            return Ok(filtered);
        }

        [Authorize]
        [HttpGet("ObtenerTipoCampana/{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var campana = await _tipoCampanaService.GetByIdAsync(id);
            return campana != null ? Ok(campana) : NoContent();
        }

        [Authorize]
        [HttpPost("CrearTipoCampana")]
        public async Task<IActionResult> AddAsync([FromBody] TipoCampana tipoCampana)
        {
            var result = await _tipoCampanaService.AddAsync(tipoCampana);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("TipoCampana:Crear", "Success"));
                return Ok(result);
            }   
        }

        [Authorize]
        [HttpPut("ActualizarTipoCampana")]
        public async Task<IActionResult> UpdateAsync([FromBody] TipoCampana tipoCampana)
        {
            var result = await _tipoCampanaService.UpdateAsync(tipoCampana);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("TipoCampana:Actualizar", "Success"));
                return Ok(result);
            }  
        }

        [Authorize]
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(Guid id)
        {
            try {
                var result = await _tipoCampanaService.Remove(id);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("TipoCampana:Eliminar", "Success"));
                    return Ok(result);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error eliminando el tipo de campaña, {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("TipoCampana:Eliminar", "Error"), id });
            } 
        } 
    } 
}