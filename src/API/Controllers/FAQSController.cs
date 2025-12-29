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
    public class FAQSController : BaseController<FAQ>, 
                                  IController<IActionResult, FAQ, Guid>
    { 
        private readonly IFAQService _faqService;
        private readonly ExportConfiguration _exportConfig;
        public FAQSController(ILogger<FAQSController> logger, 
                              IFAQService faqService, 
                              IOptions<ExportConfiguration> options) {
            _logger = logger; 
            _faqService = faqService ?? throw new ArgumentNullException(nameof(faqService));
            _exportConfig = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        //[Authorize]
        [AllowAnonymous]
        [HttpGet("ObtenerFAQS")]
        public async Task<IActionResult> GetAllAsync()
        {
            try {
                _logger.LogInformation("Obteniendo todas las FAQs.");

                var faqs = await _faqService.GetAllAsync();
                return (faqs != null && faqs.Any()) ? Ok(faqs) : NoContent();
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
            var entityName = nameof(FAQ);

            var file = await _faqService.ExportarAsync(formato);

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
                var correoN = new CorreoN {
                    Destinatario = context.Email,
                    Asunto = tipoEnvio.asunto,
                    Cuerpo = tipoEnvio.cuerpo
                };

                correoN.ApplyTags(context.GetTags());

                correoN.FicheroAdjunto = new FicheroAdjunto() {
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