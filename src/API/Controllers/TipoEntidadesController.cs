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
    public class TipoEntidadesController : BaseController<TipoEntidad>, IController<IActionResult, TipoEntidad, Guid>
    {  
        private readonly ITipoEntidadService _tipoEntidadService;

        public TipoEntidadesController(ILogger<TipoEntidadesController> logger, ITipoEntidadService tipoEntidadService)
        {
            _logger = logger; 
            _tipoEntidadService = tipoEntidadService ?? throw new ArgumentNullException(nameof(tipoEntidadService));
        }

        [Authorize]
        [HttpGet("FiltrarTipoEntidades")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] IFilters<TipoEntidad>  filters,
                                                           [FromQuery] int? page,
                                                           [FromQuery] int? pageSize,
                                                           [FromQuery] string? orderBy,
                                                           [FromQuery] bool descending = false)
        {
            var _filters = filters as TipoEntidadFilters;
            if (_filters is null)
                throw new InvalidOperationException("El filtro recibido no es 'TipoEntidadFilters'.");

            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var filtered = await _tipoEntidadService.GetByFiltersAsync(_filters, queryOptions);
            return Ok(filtered);
        }

        [Authorize]
        [HttpGet("ObtenerTipoEntidades")]
        public async Task<IActionResult> GetAllAsync()
        {
            try {
                _logger.LogInformation("Obteniendo todos los tipos de entidad."); 

                var tiposEntidad = await _tipoEntidadService.GetAllAsync();
                return (tiposEntidad != null && tiposEntidad.Any()) ? Ok(tiposEntidad) : NoContent();

            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error obteniendo todos los tipos de entidad.");
                throw ex;
            }
        }

        //[Authorize]
        //[HttpGet("ObtenerTipoEntidad/{id}")]
        //public async Task<IActionResult> GetByIdAsync(Guid id)
        //{    
        //    try {
        //        _logger.LogInformation("Obteniendo un tipo de entidad por Id.");

        //        var tipoEntidad = await _tipoEntidadService.GetByIdAsync(id);
        //        return tipoEntidad != null ? Ok(tipoEntidad) : NoContent();

        //    }
        //    catch (Exception ex)  {
        //        _logger.LogError(ex, "Error obteniendo un tipo de entidad por Id.");
        //        throw ex;
        //    } 
        //}

        //[Authorize]
        //[HttpGet("ObtenerTipoEntidadByName")]
        //public async Task<IActionResult> GetByNameAsync([FromQuery] string nombre)
        //{
        //    var tipoEntidad = await _tipoEntidadService.ObtenerPorNameAsync(nombre);
        //    return tipoEntidad != null ? Ok(tipoEntidad) : NoContent();  
        //}
        
        [Authorize]
        [HttpPost("CrearTipoEntidad")]
        public async Task<IActionResult> AddAsync([FromBody] TipoEntidad tipoEntidad)
        { 
            var result = await _tipoEntidadService.AddAsync(tipoEntidad);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("TipoEntidad:Crear", "Success"));
                return Ok(result);
            } 
        }

        [Authorize]
        [HttpPut("ActualizarTipoEntidad")]
        public async Task<IActionResult> UpdateAsync([FromBody] TipoEntidad tipoEntidad)
        {
            var result = await _tipoEntidadService.UpdateAsync(tipoEntidad);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("TipoEntidad:Actualizar", "Success"));
                return Ok(result);
            }             
        }

        [Authorize] 
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(Guid id)
        {
            try {
                var result = await _tipoEntidadService.Remove(id);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("TipoEntidad:Eliminar", "Success"));
                    return Ok(result);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error eliminando la categoria, {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("TipoEntidad:Eliminar", "Error"), id });
            } 
        }
    }
}