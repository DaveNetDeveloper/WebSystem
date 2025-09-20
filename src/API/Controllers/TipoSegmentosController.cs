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
    public class TipoSegmentosController : BaseController<TipoSegmento>, IController<IActionResult, TipoSegmento, Guid>
    { 
        private readonly ITipoSegmentoService _tipoSegmentoService;

        public TipoSegmentosController(ILogger<TipoSegmentosController> logger, ITipoSegmentoService tipoSegmentoService)
        {
            _logger = logger;
            _tipoSegmentoService = tipoSegmentoService ?? throw new ArgumentNullException(nameof(tipoSegmentoService));
        }

        [Authorize]
        [HttpGet("ObtenerTipoSegmentos")]
        public async Task<IActionResult> GetAllAsync()
        {
            var usuarioSegmentos = await _tipoSegmentoService.GetAllAsync();
            return (usuarioSegmentos != null && usuarioSegmentos.Any()) ? Ok(usuarioSegmentos) : NoContent();
        }

        [Authorize]
        [HttpGet("FiltrarTipoSegmentos")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] IFilters<TipoSegmento> filters,
                                                           [FromQuery] int? page,
                                                           [FromQuery] int? pageSize,
                                                           [FromQuery] string? orderBy,
                                                           [FromQuery] bool descending = false) {
            var _filters = filters as TipoSegmentoFilters;

            if (_filters is null)
                throw new InvalidOperationException("El filtro recibido no es 'TipoSegmentoFilters'.");

            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var filtered = await _tipoSegmentoService.GetByFiltersAsync(_filters, queryOptions);
            return Ok(filtered);
        }

        [Authorize]
        [HttpGet("ObtenerTipoSegmento/{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var entidad = await _tipoSegmentoService.GetByIdAsync(id);
            return entidad != null ? Ok(entidad) : NoContent();
        }

        [Authorize]
        [HttpPost("CrearTipoSegmento")]
        public async Task<IActionResult> AddAsync([FromBody] TipoSegmento usuarioSegmento)
        {
            var result = await _tipoSegmentoService.AddAsync(usuarioSegmento);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("TipoSegmento:Crear", "Success"));
                return Ok(result);
            }   
        }

        [Authorize]
        [HttpPut("ActualizarTipoSegmento")]
        public async Task<IActionResult> UpdateAsync([FromBody] TipoSegmento usuarioSegmento)
        {
            var result = await _tipoSegmentoService.UpdateAsync(usuarioSegmento);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("TipoSegmento:Actualizar", "Success"));
                return Ok(result);
            }  
        }

        [Authorize]
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(Guid id)
        {
            try {
                var result = await _tipoSegmentoService.Remove(id);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("TipoSegmento:Eliminar", "Success"));
                    return Ok(result);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error eliminando el tipo de segmento, {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("TipoSegmento:Eliminar", "Error"), id });
            } 
        } 
    } 
}