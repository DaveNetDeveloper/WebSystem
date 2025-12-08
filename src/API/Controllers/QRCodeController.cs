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
using Microsoft.Extensions.Options;
using static Utilities.ExporterHelper;

namespace API.Controllers
{
    [ApiController]
    [Route("api/qr")]
    public class QRCodesController : BaseController<QRCode>
    {
        private readonly IQRCodeService _service;
        private readonly ExportConfiguration _exportConfig;

        public QRCodesController(IQRCodeService service,
                                ILogger<QRCodesController> logger,
                                   IOptions<ExportConfiguration> options)
        {
            _service = service;
            _logger = logger;
            _exportConfig = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        [AllowAnonymous]
        //[Authorize]
        [HttpGet("ObtenerQRs")]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var productos = await _service.GetAllAsync();
                return (productos != null && productos.Any()) ? Ok(productos) : NoContent();
            }
            catch (Exception ex)
            {
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
            try
            {
                var result = await _service.Remove(id);
                if (result == false) return NotFound();
                return Ok("QR consumido correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("QR:Eliminar", "Error"), id });
            }
        }

        /// <summary>
        /// Exportar listado a Excel o pdf
        /// </summary> 
        /// <returns> File to download </returns>
        [HttpGet("Exportar")]
        //[Authorize(Policy = "RequireAdmin")]
        //[Authorize]
        [AllowAnonymous]
        public async Task<IActionResult> Exportar([FromServices] ICorreoService correoService,
                                                  [FromQuery] ExportFormat formato,
                                                  [FromQuery] bool envioEmail)
        {
            var entityName = nameof(QRCode);

            var file = await _service.ExportarAsync(formato);

            string fileExtension = string.Empty;
            string contentType = string.Empty;

            switch (formato)
            {
                case ExportFormat.Excel:
                    contentType = _exportConfig.ExcelContentType;
                    fileExtension = _exportConfig.ExcelExtension;
                    break;
                case ExportFormat.Pdf:
                    contentType = _exportConfig.PdfContentType;
                    fileExtension = _exportConfig.PdfExtension;
                    break;
            }

            var fileName = $"List_{entityName.ToString()}_{DateTime.UtcNow:yyyyMMddHHmmss}{fileExtension}";

            if (envioEmail)
            {
                var tiposEnvioCorreo = await correoService.ObtenerTiposEnvioCorreo();
                var tipoEnvioCorreo = tiposEnvioCorreo.Where(u => u.nombre == TipoEnvioCorreo.TipoEnvio.EnvioReport)
                                                      .SingleOrDefault();

                tipoEnvioCorreo.asunto = $"Report {entityName.ToString()} ({fileExtension})";
                tipoEnvioCorreo.cuerpo = $"Se adjunta el informe para la vista de datos {entityName.ToString()}";

                var correo = new Correo(tipoEnvioCorreo, _exportConfig.CorreoAdmin, "Admin", "");
                correo.FicheroAdjunto = new FicheroAdjunto()
                {
                    Archivo = file,
                    ContentType = contentType,
                    NombreArchivo = fileName
                };
                correoService.EnviarCorreo(correo);
            }
            return File(file, contentType, fileName);
        }
    }
}
