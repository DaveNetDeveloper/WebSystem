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
using static Domain.Entities.TipoEnvioCorreo;
using static Utilities.ExporterHelper;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestimoniosController : BaseController<Testimonio>, IController<IActionResult, Testimonio, int>
    { 
        private readonly ITestimonioService _testimonioService;
        private readonly ExportConfiguration _exportConfig;

        public TestimoniosController(ILogger<TestimoniosController> logger,
                                     ITestimonioService testimonioService,
                                     IOptions<ExportConfiguration> options)
        {
            _logger = logger; 
            _testimonioService = testimonioService ?? throw new ArgumentNullException(nameof(testimonioService));
            _exportConfig = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        //[Authorize]
        [AllowAnonymous]
        [HttpGet("FiltrarTestimonios")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] TestimonioFilters filters,
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

        //[Authorize]
        [AllowAnonymous]
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
            var entityName = nameof(Testimonio);

            var file = await _testimonioService.ExportarAsync(formato);

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