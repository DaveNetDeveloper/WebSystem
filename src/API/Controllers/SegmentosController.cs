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
    public class SegmentosController : BaseController<Segmento>, IController<IActionResult, Segmento, int>
    { 
        private readonly ISegmentoService _segmentoService;

        public SegmentosController(ILogger<SegmentosController> logger, ISegmentoService segmentoService)
        {
            _logger = logger;
            _segmentoService = segmentoService ?? throw new ArgumentNullException(nameof(segmentoService));
        }

        [Authorize]
        [HttpGet("ObtenerSegmentos")]
        public async Task<IActionResult> GetAllAsync()
        {
            var segmentos = await _segmentoService.GetAllAsync();
            return (segmentos != null && segmentos.Any()) ? Ok(segmentos) : NoContent();
        }

        [Authorize]
        [HttpGet("FiltrarSegmentos")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] IFilters<Segmento> filters,
                                                           [FromQuery] int? page,
                                                           [FromQuery] int? pageSize,
                                                           [FromQuery] string? orderBy,
                                                           [FromQuery] bool descending = false) {
            var _filters = filters as SegmentoFilters;

            if (_filters is null)
                throw new InvalidOperationException("El filtro recibido no es 'SegmentoFilters'.");

            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var filtered = await _segmentoService.GetByFiltersAsync(_filters, queryOptions);
            return Ok(filtered);
        }

        [Authorize]
        [HttpGet("ObtenerSegmento/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var entidad = await _segmentoService.GetByIdAsync(id);
            return entidad != null ? Ok(entidad) : NoContent();
        }

        [Authorize]
        [HttpPost("CrearSegmento")]
        public async Task<IActionResult> AddAsync([FromBody] Segmento segmento)
        {
            var result = await _segmentoService.AddAsync(segmento);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("Segmento:Crear", "Success"));
                return Ok(result);
            }   
        }

        [Authorize]
        [HttpPut("ActualizarSegmento")]
        public async Task<IActionResult> UpdateAsync([FromBody] Segmento segmento)
        {
            var result = await _segmentoService.UpdateAsync(segmento);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("Segmento:Actualizar", "Success"));
                return Ok(result);
            }  
        }

        [Authorize]
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            try {
                var result = await _segmentoService.Remove(id);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("Segmento:Eliminar", "Success"));
                    return Ok(result);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error eliminando el segmento: {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("Segmento:Eliminar", "Error"), id });
            } 
        }
         
    } 
}