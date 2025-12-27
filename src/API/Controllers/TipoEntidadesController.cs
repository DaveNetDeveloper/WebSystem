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
    public class TipoEntidadesController : BaseController<TipoEntidad>, IController<IActionResult, TipoEntidad, Guid>
    {  
        private readonly ITipoEntidadService _tipoEntidadService;
        private readonly ExportConfiguration _exportConfig;

        public TipoEntidadesController(ILogger<TipoEntidadesController> logger, 
                                       ITipoEntidadService tipoEntidadService,
                                       IOptions<ExportConfiguration> options) {
            _logger = logger; 
            _tipoEntidadService = tipoEntidadService ?? throw new ArgumentNullException(nameof(tipoEntidadService));
            _exportConfig = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        [AllowAnonymous]
        //[Authorize]
        [HttpGet("FiltrarTipoEntidades")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] IFilters<TipoEntidad>  filters,
                                                           [FromQuery] int? page,
                                                           [FromQuery] int? pageSize,
                                                           [FromQuery] string? orderBy,
                                                           [FromQuery] bool descending = false)
        {
            var _filters = filters as TipoEntidadFilters;
            if (_filters is null)
                throw new InvalidOperationException("El filtro recibido no es 'TipoEntidadFilters'.");

            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var filtered = await _tipoEntidadService.GetByFiltersAsync(_filters, queryOptions);
            return Ok(filtered);
        }

        [AllowAnonymous]
        //[Authorize]
        [HttpGet("ObtenerTipoEntidades")]
        public async Task<IActionResult> GetAllAsync()
        {
            try {
                _logger.LogInformation("Obteniendo todos los tipos de entidad."); 

                var tiposEntidad = await _tipoEntidadService.GetAllAsync();
                return (tiposEntidad != null && tiposEntidad.Any()) ? Ok(tiposEntidad) : NoContent();

            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error obteniendo todos los tipos de entidad.");
                throw ex;
            }
        }

        //[Authorize]
        //[HttpGet("ObtenerTipoEntidad/{id}")]
        //public async Task<IActionResult> GetByIdAsync(Guid id)
        //{    
        //    try {
        //        _logger.LogInformation("Obteniendo un tipo de entidad por Id.");

        //        var tipoEntidad = await _tipoEntidadService.GetByIdAsync(id);
        //        return tipoEntidad != null ? Ok(tipoEntidad) : NoContent();

        //    }
        //    catch (Exception ex)  {
        //        _logger.LogError(ex, "Error obteniendo un tipo de entidad por Id.");
        //        throw ex;
        //    } 
        //}
         
        
        [Authorize]
        [HttpPost("CrearTipoEntidad")]
        public async Task<IActionResult> AddAsync([FromBody] TipoEntidad tipoEntidad)
        { 
            var result = await _tipoEntidadService.AddAsync(tipoEntidad);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("TipoEntidad:Crear", "Success"));
                return Ok(result);
            } 
        }

        [Authorize]
        [HttpPut("ActualizarTipoEntidad")]
        public async Task<IActionResult> UpdateAsync([FromBody] TipoEntidad tipoEntidad)
        {
            var result = await _tipoEntidadService.UpdateAsync(tipoEntidad);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("TipoEntidad:Actualizar", "Success"));
                return Ok(result);
            }             
        }

        [Authorize] 
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(Guid id)
        {
            try {
                var result = await _tipoEntidadService.Remove(id);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("TipoEntidad:Eliminar", "Success"));
                    return Ok(result);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error eliminando la categoria, {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("TipoEntidad:Eliminar", "Error"), id });
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
            var entityName = nameof(TipoEntidad);

            var file = await _tipoEntidadService.ExportarAsync(formato);

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