using Application.DTOs.Filters;
using Application.Interfaces.Controllers;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Services;
using Application.Services;
using Domain.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using static Domain.Entities.TipoEnvioCorreo;
using static Utilities.ExporterHelper;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TipoActividadesController : BaseController<TipoActividad>, IController<IActionResult, TipoActividad, Guid>
    {  
        private readonly ITipoActividadService _tipoActividadService;
        private readonly ExportConfiguration _exportConfig;

        public TipoActividadesController(ILogger<TipoActividadesController> logger, 
                                         ITipoActividadService tipoActividaddService,
                                         IOptions<ExportConfiguration> options)
        {
            _logger = logger; 
            _tipoActividadService = tipoActividaddService ?? throw new ArgumentNullException(nameof(tipoActividaddService));
            _exportConfig = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        [AllowAnonymous]
        //[Authorize]
        [HttpGet("FiltrarTipoActividades")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] TipoActividadFilters  filters,
                                                           [FromQuery] int? page,
                                                           [FromQuery] int? pageSize,
                                                           [FromQuery] string? orderBy,
                                                           [FromQuery] bool descending = false)
        {
            var _filters = filters as TipoActividadFilters;
            if (_filters is null)
                throw new InvalidOperationException("El filtro recibido no es 'TipoActividadFilters'.");

            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var filtered = await _tipoActividadService.GetByFiltersAsync(_filters, queryOptions);
            return Ok(filtered);
        }

        [AllowAnonymous]
        //[Authorize]
        [HttpGet("ObtenerTipoActividades")]
        public async Task<IActionResult> GetAllAsync()
        {
            try {
                _logger.LogInformation("Obteniendo todos los tipos de actividad."); 

                var tiposActividad = await _tipoActividadService.GetAllAsync();
                return (tiposActividad != null && tiposActividad.Any()) ? Ok(tiposActividad) : NoContent();

            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error obteniendo todos los tipos de actividad.");
                throw ex;
            }
        }

        [Authorize]
        [HttpGet("ObtenerTipoActividad/{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Obteniendo un tipo de actividad por Id.");

                var tipoActividad = await _tipoActividadService.GetByIdAsync(id);
                return tipoActividad != null ? Ok(tipoActividad) : NoContent();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo un tipo de actividad> por Id.");
                throw ex;
            }
        }

        [Authorize]
        [HttpPost("CrearTipoActividad")]
        public async Task<IActionResult> AddAsync([FromBody] TipoActividad tipoActividad)
        { 
            var result = await _tipoActividadService.AddAsync(tipoActividad);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("TipoActividad:Crear", "Success"));
                return Ok(result);
            } 
        }

        [Authorize]
        [HttpPut("ActualizarTipoActividad")]
        public async Task<IActionResult> UpdateAsync([FromBody] TipoActividad tipoActividad)
        {
            var result = await _tipoActividadService.UpdateAsync(tipoActividad);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("TipoActividad:Actualizar", "Success"));
                return Ok(result);
            }             
        }

        [Authorize] 
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(Guid id)
        {
            try {
                var result = await _tipoActividadService.Remove(id);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("TipoActividad:Eliminar", "Success"));
                    return Ok(result);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error eliminando el tipo de actividad, {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("TipoActividad:Eliminar", "Error"), id });
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
            var entityName = nameof(TipoActividad);

            var file = await _tipoActividadService.ExportarAsync(formato);

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