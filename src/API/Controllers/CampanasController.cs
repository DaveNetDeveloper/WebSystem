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
    public class CampanasController : BaseController<Campana>, IController<IActionResult, Campana, int>
    {  
        private readonly ICampanaService _campanaService;

        public CampanasController(ILogger<CampanasController> logger, ICampanaService campanaService) {
            _logger = logger;
            _campanaService = campanaService ?? throw new ArgumentNullException(nameof(campanaService));
        }

        [Authorize]
        [HttpGet("ObtenerCampanas")]
        public async Task<IActionResult> GetAllAsync()
        {
            try {
                _logger.LogInformation("Obteniendo todos las campañas."); 

                var campanas = await _campanaService.GetAllAsync();
                if (campanas == null || !campanas.Any()) return NoContent();  

                return Ok(campanas);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error obteniendo todas las campañas.");
                return StatusCode(StatusCodes.Status500InternalServerError, 
                                  new { message = "Se produjo un error al obtener las campañas." });
            } 
        }

        [Authorize]
        [HttpGet("FiltrarCampanas")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] IFilters<Campana> filters,
                                                           [FromQuery] int? page,
                                                           [FromQuery] int? pageSize,
                                                           [FromQuery] string? orderBy,
                                                           [FromQuery] bool descending = false) { 
            var _filters = filters as CampanaFilters;

            if (_filters is null)
                throw new InvalidOperationException("El filtro recibido no es 'CampanaFilters'.");

            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var filtered = await _campanaService.GetByFiltersAsync(_filters, queryOptions);
            return Ok(filtered);
        }

        //[Authorize]
        //[HttpGet("ObtenerCampana/{id}")]
        //public async Task<IActionResult> GetByIdAsync(int id)
        //{
        //    try
        //    {
        //        _logger.LogInformation("Obteniendo una campaña por Id.");

        //        var campana = await _campanaService.GetByIdAsync(id);
        //        if (campana == null) return NoContent();

        //        return Ok(campana);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error obteniendo una campana por Id, {id}.", id);
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //                         new { message = "Se produjo un error al obtener una campana por el Id.", id });
        //    }
        //}

        [Authorize]
        [HttpPost("CrearCampana")]
        public async Task<IActionResult> AddAsync([FromBody] Campana campana)
        {
            try {
                _logger.LogInformation("Creando una nueva campaña.");

                var result = await _campanaService.AddAsync(campana);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("Campana:Crear", "Success"));
                    return Ok(result);
                }     
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error creando una nueva campaña.");
                
            }
            return NoContent();
        }

        [Authorize]
        [HttpPut("ActualizarCampana")]
        public async Task<IActionResult> UpdateAsync([FromBody] Campana campana)
        {
            try {
                _logger.LogInformation("Actualizando una campaña.");

                var result = await _campanaService.UpdateAsync(campana);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("Campana:Actualizar", "Success"));
                    return Ok(result);
                } 
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error actualizando una campana.");
               
            }
            return NoContent();
        }

        [Authorize]
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            try {
                _logger.LogInformation("Eliminando una campaña por Id.");

                var result = await _campanaService.Remove(id);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("Campana:Eliminar", "Success"));
                    return Ok(result);
                }  
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error eliminando una campaña por Id.");
              
            }
            return NoContent(); 
        }

        //[Authorize]
        //[HttpGet("GetImagenesByProducto/{id}")]
        //public async Task<IActionResult> GetImagenesByProducto(int id)
        //{
        //    try { 
        //        var imagenesByProduct = await _productoService.GetImagenesByProducto(id);

        //        if (imagenesByProduct == null || !imagenesByProduct.Any()) return NotFound();
        //        else {
        //            //_logger.LogInformation(MessageProvider.GetMessage("Producto:GetImagenes", "Success"));
        //            return Ok(imagenesByProduct);
        //        }
        //    }
        //    catch (Exception ex) {
        //        _logger.LogError(ex, "Error obteniendo las imagenes del producto {id}.", id);
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //                         new { message = MessageProvider.GetMessage("Producto:GetImagenes", "Error"), id });
        //    }
        //} 

    }
}