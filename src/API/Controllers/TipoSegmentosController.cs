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
    public class TipoSegmentosController : BaseController<TipoSegmento>, IController<IActionResult, TipoSegmento, Guid>
    { 
        private readonly ITipoSegmentoService _tipoSegmentoService;
        private readonly ExportConfiguration _exportConfig;

        public TipoSegmentosController(ILogger<TipoSegmentosController> logger, 
                                       ITipoSegmentoService tipoSegmentoService, 
                                       IOptions<ExportConfiguration> options)
        {
            _logger = logger;
            _tipoSegmentoService = tipoSegmentoService ?? throw new ArgumentNullException(nameof(tipoSegmentoService));
            _exportConfig = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        [AllowAnonymous]
        //[Authorize]
        [HttpGet("ObtenerTipoSegmentos")]
        public async Task<IActionResult> GetAllAsync()
        {
            var tipoSegmentos = await _tipoSegmentoService.GetAllAsync();
            return (tipoSegmentos != null && tipoSegmentos.Any()) ? Ok(tipoSegmentos) : NoContent();
        }

        [Authorize]
        [HttpGet("FiltrarTipoSegmentos")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] IFilters<TipoSegmento> filters,
                                                           [FromQuery] int? page,
                                                           [FromQuery] int? pageSize,
                                                           [FromQuery] string? orderBy,
                                                           [FromQuery] bool descending = false) {
            var _filters = filters as TipoSegmentoFilters;

            if (_filters is null)
                throw new InvalidOperationException("El filtro recibido no es 'TipoSegmentoFilters'.");

            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var filtered = await _tipoSegmentoService.GetByFiltersAsync(_filters, queryOptions);
            return Ok(filtered);
        }

        [Authorize]
        [HttpGet("ObtenerTipoSegmento/{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var entidad = await _tipoSegmentoService.GetByIdAsync(id);
            return entidad != null ? Ok(entidad) : NoContent();
        }

        [Authorize]
        [HttpPost("CrearTipoSegmento")]
        public async Task<IActionResult> AddAsync([FromBody] TipoSegmento tipoSegmento)
        {
            var result = await _tipoSegmentoService.AddAsync(tipoSegmento);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("TipoSegmento:Crear", "Success"));
                return Ok(result);
            }   
        }

        [Authorize]
        [HttpPut("ActualizarTipoSegmento")]
        public async Task<IActionResult> UpdateAsync([FromBody] TipoSegmento tipoSegmento)
        {
            var result = await _tipoSegmentoService.UpdateAsync(tipoSegmento);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("TipoSegmento:Actualizar", "Success"));
                return Ok(result);
            }  
        }

        [Authorize]
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(Guid id)
        {
            try {
                var result = await _tipoSegmentoService.Remove(id);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("TipoSegmento:Eliminar", "Success"));
                    return Ok(result);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error eliminando el tipo de segmento, {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("TipoSegmento:Eliminar", "Error"), id });
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
            var entityName = nameof(TipoSegmento);

            var file = await _tipoSegmentoService.ExportarAsync(formato);

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