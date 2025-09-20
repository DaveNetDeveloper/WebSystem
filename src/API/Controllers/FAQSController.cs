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
    public class FAQSController : BaseController<FAQ>, IController<IActionResult, FAQ, Guid>
    { 
        private readonly IFAQService _faqService;
        public FAQSController(ILogger<FAQSController> logger, IFAQService faqService) {
            _logger = logger; 
            _faqService = faqService ?? throw new ArgumentNullException(nameof(faqService)); 
        }

        [Authorize]
        [HttpGet("ObtenerFAQS")]
        public async Task<IActionResult> GetAllAsync()
        {
            try {
                _logger.LogInformation("Obteniendo todas las FAQs.");

                var productos = await _faqService.GetAllAsync();
                return (productos != null && productos.Any()) ? Ok(productos) : NoContent();
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error obteniendo todas las FAQs.");
                return NoContent();
            }
        }

        [Authorize]
        [HttpGet("FiltrarFAQS")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] IFilters<FAQ> filters,
                                                           [FromQuery] int? page,
                                                           [FromQuery] int? pageSize,
                                                           [FromQuery] string? orderBy,
                                                           [FromQuery] bool descending = false) {
            var _filters = filters as FAQFilters;

            if (_filters is null)
                throw new InvalidOperationException("El filtro recibido no es 'FAQFilters'.");

            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var filtered = await _faqService.GetByFiltersAsync(_filters, queryOptions);
            return Ok(filtered);
        }

        //[Authorize]
        //[HttpGet("ObtenerFAQ/{id}")]
        //public async Task<IActionResult> GetByIdAsync(Guid id)
        //{
        //    try {
        //        _logger.LogInformation("Obteniendo una FAQ por Id.");

        //        var producto = await _faqService.GetByIdAsync(id);
        //        return producto != null ? Ok(producto) : NoContent();
        //    }
        //    catch (Exception ex) {
        //        _logger.LogError(ex, "Error obteniendo una FAQ por Id.");
        //        return NoContent();
        //    }
        //}  

        [Authorize]
        [HttpPost("CrearFAQ")]
        public async Task<IActionResult> AddAsync([FromBody] FAQ faq)
        {
            var result = await _faqService.AddAsync(faq);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("FAQ:Crear", "Success"));
                return Ok(result);
            } 
        }

        [Authorize]
        [HttpPut("ActualizarFAQ")]
        public async Task<IActionResult> UpdateAsync([FromBody] FAQ faq)
        {
            var result = await _faqService.UpdateAsync(faq);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("FAQ:Actualizar", "Success"));
                return Ok(result);
            } 
        }

        [Authorize]
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(Guid id)
        { 
            try {
                var result = await _faqService.Remove(id);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("FAQ:Eliminar", "Success"));
                    return Ok(result);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error eliminando la FAQ, {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("FAQ:Eliminar", "Error"), id });
            } 
        }
    }
}