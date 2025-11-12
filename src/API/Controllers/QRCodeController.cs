using Application.Common;
using Application.DTOs.Filters;
using Application.DTOs.Requests;
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
    [Route("api/qr")]
    public class QRCodeController : BaseController<QRCode>
    {
        private readonly QRCodeService _service;

        public QRCodeController(QRCodeService service, 
                                ILogger<QRCodeController> logger) {
            _service = service;
            _logger = logger;
        }

        [Authorize]
        [HttpGet("ObtenerQRs")]
        public async Task<IActionResult> GetAllAsync()
        {
            try  { 
                var productos = await _service.GetAllAsync();
                return (productos != null && productos.Any()) ? Ok(productos) : NoContent();
            }
            catch (Exception ex) { 
                return NoContent();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateQRCodeRequest request)
        {
            var qr = await _service.CreateAsync(
                request.Payload,
                request.Ttl.HasValue ? TimeSpan.FromSeconds(request.Ttl.Value) : null
            );

            return Ok(new QRCodeResponse(qr));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var qr = await _service.GetAsync(id);
            if (qr == null) return NotFound();
            return Ok(new QRCodeResponse(qr));
        }

        [HttpGet("{id}/image")]
        public async Task<IActionResult> GetImage(Guid id)
        {
            var qr = await _service.GetAsync(id);
            if (qr == null) return NotFound();
            return File(qr.imagen!, "image/png");
        }

        [HttpPost("{id}/activate")]
        public async Task<IActionResult> Activate(Guid id)
        {
            await _service.ActivateAsync(id);
            return Ok("QR activado correctamente");
        }

        [HttpPost("{id}/deactivate")]
        public async Task<IActionResult> Deactivate(Guid id)
        {
            await _service.DeactivateAsync(id);
            return Ok("QR desactivado correctamente");
        }

        [HttpPost("{id}/consume")]
        public async Task<IActionResult> Consume(Guid id)
        {
            await _service.ConsumeAsync(id);
            return Ok("QR consumido correctamente");
        }

        [Authorize]
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(Guid id)
        {
            try {
                var result = await _service.Remove(id);
                if (result == false) return NotFound();
                return Ok("QR consumido correctamente");
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("QR:Eliminar", "Error"), id });
            }
        }

    }
}
