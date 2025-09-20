using Application.Common;
using Application.DTOs.Filters;
using Application.Interfaces;
using Application.Interfaces.Controllers;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Services;
using Domain.Entities; 

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QRsController : BaseController<QR>, IController<IActionResult, QR, Guid>
    {
        private readonly IQRService _qrService;

        public QRsController(ILogger<QRsController> logger, IQRService qrService)
        {
            _logger = logger;
            _qrService = qrService ?? throw new ArgumentNullException(nameof(qrService));
        }

        [Authorize]
        [HttpGet("FiltrarQRs")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] IFilters<QR> filters,
                                                           [FromQuery] int? page,
                                                           [FromQuery] int? pageSize,
                                                           [FromQuery] string? orderBy,
                                                           [FromQuery] bool descending = false) {
            var _filters = filters as QRFilters; 
            if (_filters is null)
                throw new InvalidOperationException("El filtro recibido no es 'QRFilters'.");

            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var filtered = await _qrService.GetByFiltersAsync(_filters, queryOptions);
            return Ok(filtered);
        }

        [Authorize]
        [HttpGet("ObtenerQRs")]
        public async Task<IActionResult> GetAllAsync()
        {
            try {
                _logger.LogInformation("Obteniendo todos los QRs.");

                var productos = await _qrService.GetAllAsync();
                return (productos != null && productos.Any()) ? Ok(productos) : NoContent();
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error obteniendo todos los QRs.");
                return NoContent();
            }
        }

        //[Authorize]
        //[HttpGet("ObtenerQR/{id}")]
        //public async Task<IActionResult> GetByIdAsync(Guid id)
        //{
        //    try {
        //        _logger.LogInformation("Obteniendo un QR por Id.");

        //        var qr = await _qrService.GetByIdAsync(id);
        //        return qr != null ? Ok(qr) : NoContent();
        //    }
        //    catch (Exception ex) {
        //        _logger.LogError(ex, "Error obteniendo un QR por Id.");
        //        return NoContent();
        //    } 
        //}

        [Authorize]
        [HttpPost("CrearQR")]
        public async Task<IActionResult> AddAsync([FromBody] QR qr)
        {
            var result = await _qrService.AddAsync(qr);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("QR:Crear", "Success"));
                return Ok(result);
            } 
        }

        [Authorize]
        [HttpPut("ActualizarQR")]
        public async Task<IActionResult> UpdateAsync([FromBody] QR qr)
        {
            var result = await _qrService.UpdateAsync(qr);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("QR:Actualizar", "Success"));
                return Ok(result);
            } 
        }

        [Authorize]
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(Guid id)
        {
            try {
                var result = await _qrService.Remove(id);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("QR:Eliminar", "Success"));
                    return Ok(result);
                }
            }
            catch (Exception ex)  {
                _logger.LogError(ex, "Error eliminando el QR, {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("QR:Eliminar", "Error"), id });
            } 
        }
    }
}