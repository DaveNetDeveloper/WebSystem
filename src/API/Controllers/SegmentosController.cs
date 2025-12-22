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
using static Utilities.ExporterHelper;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SegmentosController : BaseController<Segmento>, IController<IActionResult, Segmento, int>
    { 
        private readonly ISegmentoService _segmentoService;
        private readonly ExportConfiguration _exportConfig;

        public SegmentosController(ILogger<SegmentosController> logger, ISegmentoService segmentoService,
                                   IOptions<ExportConfiguration> options)
        {
            _logger = logger;
            _segmentoService = segmentoService ?? throw new ArgumentNullException(nameof(segmentoService));
            _exportConfig = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        //[Authorize]
        [HttpGet("ObtenerSegmentos")]
        public async Task<IActionResult> GetAllAsync()
        {
            var segmentos = await _segmentoService.GetAllAsync();
            return (segmentos != null && segmentos.Any()) ? Ok(segmentos) : NoContent();
        }

        [Authorize]
        [HttpGet("FiltrarSegmentos")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] IFilters<Segmento> filters,
                                                           [FromQuery] int? page,
                                                           [FromQuery] int? pageSize,
                                                           [FromQuery] string? orderBy,
                                                           [FromQuery] bool descending = false) {
            var _filters = filters as SegmentoFilters;

            if (_filters is null)
                throw new InvalidOperationException("El filtro recibido no es 'SegmentoFilters'.");

            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var filtered = await _segmentoService.GetByFiltersAsync(_filters, queryOptions);
            return Ok(filtered);
        }

        [Authorize]
        [HttpGet("ObtenerSegmento/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var entidad = await _segmentoService.GetByIdAsync(id);
            return entidad != null ? Ok(entidad) : NoContent();
        }

        [Authorize]
        [HttpPost("CrearSegmento")]
        public async Task<IActionResult> AddAsync([FromBody] Segmento segmento)
        {
            var result = await _segmentoService.AddAsync(segmento);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("Segmento:Crear", "Success"));
                return Ok(result);
            }   
        }

        [Authorize]
        [HttpPut("ActualizarSegmento")]
        public async Task<IActionResult> UpdateAsync([FromBody] Segmento segmento)
        {
            var result = await _segmentoService.UpdateAsync(segmento);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("Segmento:Actualizar", "Success"));
                return Ok(result);
            }  
        }

        [Authorize]
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            try {
                var result = await _segmentoService.Remove(id);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("Segmento:Eliminar", "Success"));
                    return Ok(result);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error eliminando el segmento: {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("Segmento:Eliminar", "Error"), id });
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
            var entityName = nameof(Segmento);

            var file = await _segmentoService.ExportarAsync(formato);

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