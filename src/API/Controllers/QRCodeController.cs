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
    public class QRCodeController : ControllerBase
    {
        private readonly QRCodeService _service;

        public QRCodeController(QRCodeService service) => _service = service;

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
    }
}
