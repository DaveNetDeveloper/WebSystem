using Application.Common;
using Application.DTOs.Filters;
using Application.Interfaces.Controllers;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Services;
using Application.Services;
using Domain.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using static Domain.Entities.TipoEnvioCorreo;
using static Domain.Entities.Transaccion;
using static Utilities.ExporterHelper;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TipoRecompensasController : BaseController<TipoRecompensa>, 
                                             IController<IActionResult, TipoRecompensa, Guid>
    { 
        private readonly IRecompensaService _tipoRecompensaService;
        private readonly ExportConfiguration _exportConfig;

        public TipoRecompensasController(ILogger<TipoRecompensasController> logger,
                                         IRecompensaService tipoTransaccionService,
                                         IOptions<ExportConfiguration> options)
        {
            _logger = logger;
            _tipoRecompensaService = tipoTransaccionService ?? throw new ArgumentNullException(nameof(tipoTransaccionService));
            _exportConfig = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        [AllowAnonymous]
        //[Authorize]
        [HttpGet("ObtenerTipoRecompensas")]
        public async Task<IActionResult> GetAllAsync()
        {
            var tipoRecompensas = await _tipoRecompensaService.GetAllTiposRecompensas();
            return (tipoRecompensas != null && tipoRecompensas.Any()) ? Ok(tipoRecompensas) : NoContent();
        }

        [Authorize]
        [HttpGet("ObtenerTipoRecompensa/{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var tipoRecompensa = await _tipoRecompensaService.GetTipoRecompensa(id);
            return tipoRecompensa != null ? Ok(tipoRecompensa) : NoContent();
        }

        [Authorize]
        [HttpPost("CrearTipoRecompensa")]
        public async Task<IActionResult> AddAsync([FromBody] TipoRecompensa tipoRecompensa)
        {
            var result = await _tipoRecompensaService.AddTipoRecompensa(tipoRecompensa);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("TipoRecompensa:Crear", "Success"));
                return Ok(result);
            }   
        }

        [Authorize]
        [HttpPut("ActualizarTipoRecompensa")]
        public async Task<IActionResult> UpdateAsync([FromBody] TipoRecompensa tipoRecompensa)
        {
            var result = await _tipoRecompensaService.UpdateTipoRecompensa(tipoRecompensa);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("TipoRecompensa:Actualizar", "Success"));
                return Ok(result);
            }  
        }

        [Authorize]
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(Guid id)
        {
            try {
                var result = await _tipoRecompensaService.RemoveTipoRecompensa(id);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("TipoRecompensa:Eliminar", "Success"));
                    return Ok(result);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error eliminando el tipo de recompensa, {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("TipoRecompensa:Eliminar", "Error"), id });
            } 
        }

        /// <summary>
        /// Exportar vista a Excel o pdf
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
            var entityName = nameof(TipoRecompensa);

            var file = await _tipoRecompensaService.ExportarAsync(formato);

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
                var tipoEnvio = await correoService.ObtenerTipoEnvioCorreo(TipoEnvioCorreos.EnvioReport);

                var context = new EnvioReportEmailContext(email: _exportConfig.CorreoAdmin,
                                                          nombre: "Admin",
                                                          nombreEntidad: "",
                                                          nombreInforme: $"List_{entityName.ToString()}");
                var correoN = new CorreoN
                {
                    Destinatario = context.Email,
                    Asunto = tipoEnvio.asunto,
                    Cuerpo = tipoEnvio.cuerpo
                };

                correoN.ApplyTags(context.GetTags());

                correoN.FicheroAdjunto = new FicheroAdjunto()
                {
                    Archivo = file,
                    ContentType = contentType,
                    NombreArchivo = fileName
                };
                correoService.EnviarCorreo_Nuevo(correoN);
            }
            return File(file, contentType, fileName);
        }
    } 
}