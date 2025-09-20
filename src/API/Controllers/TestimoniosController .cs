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
    public class TestimoniosController : BaseController<Testimonio>, IController<IActionResult, Testimonio, int>
    { 
        private readonly ITestimonioService _testimonioService;
         
        public TestimoniosController(ILogger<TestimoniosController> logger, ITestimonioService testimonioService) {
            _logger = logger; 
            _testimonioService = testimonioService ?? throw new ArgumentNullException(nameof(testimonioService));
        }

        [Authorize]
        [HttpGet("FiltrarTestimonios")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] IFilters<Testimonio>  filters,
                                                           [FromQuery] int? page,
                                                           [FromQuery] int? pageSize,
                                                           [FromQuery] string? orderBy,
                                                           [FromQuery] bool descending = false) {
            var _filters = filters as TestimonioFilters;
            if (_filters is null)
                throw new InvalidOperationException("El filtro recibido no es 'TestimonioFilters'.");

            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var filtered = await _testimonioService.GetByFiltersAsync(_filters, queryOptions);
            return Ok(filtered);
        }

        [Authorize]
        [HttpGet("ObtenerTestimonios")]
        public async Task<IActionResult> GetAllAsync()
        {
            try  {
                _logger.LogInformation("Obteniendo todos los testimonios.");

                var testimonios = await _testimonioService.GetAllAsync();
                return (testimonios != null && testimonios.Any()) ? Ok(testimonios) : NoContent(); 
            }
            catch (Exception ex)  {
                _logger.LogError(ex, "Error obteniendo todos los testimonios.");
                throw ex;
            } 
        }

        //[Authorize]
        //[HttpGet("ObtenerTestimonio/{id}")]
        //public async Task<IActionResult> GetByIdAsync(int id)
        //{ 
        //    try {
        //        _logger.LogInformation("Obteniendo un testimonio por Id.");

        //        var testimonio = await _testimonioService.GetByIdAsync(id);
        //        return testimonio != null ? Ok(testimonio) : NoContent();
        //    }
        //    catch (Exception ex) {
        //        _logger.LogError(ex, "Error obteniendo un testimonio por Id.");
        //        throw ex;
        //    }
        //}

        [Authorize]
        [HttpPost("CrearTestimonio")]
        public async Task<IActionResult> AddAsync([FromBody] Testimonio testimonio)
        {
            var result = await _testimonioService.AddAsync(testimonio);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("Testimonio:Crear", "Success"));
                return Ok(result);
            }
        }

        [Authorize]
        [HttpPut("ActualizarTestimonio")]
        public async Task<IActionResult> UpdateAsync([FromBody] Testimonio testimonio)
        {
            var result = await _testimonioService.UpdateAsync(testimonio);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("Testimonio:Actualizar", "Success"));
                return Ok(result);
            }
        }

        [Authorize]
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            try {
                var result = await _testimonioService.Remove(id);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("Testimonio:Eliminar", "Success"));
                    return Ok(result);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error eliminando el testimonio, {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("Testimonio:Eliminar", "Error"), id });
            } 
        }
    }
}