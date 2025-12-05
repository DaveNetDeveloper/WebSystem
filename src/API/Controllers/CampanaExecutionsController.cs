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
using static Utilities.ExporterHelper;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CampanaExecutionsController : BaseController<CampanaExecution>,
                                               IController<IActionResult, CampanaExecution, Guid>
    {  
        private readonly ICampanaExecutionService _campanaExecutionService;
        private readonly ExportConfiguration _exportConfig;

        public CampanaExecutionsController(ILogger<CampanaExecutionsController> logger, 
                                           ICampanaExecutionService campanaExecutionService,
                                           IOptions<ExportConfiguration> options)
        {
            _logger = logger;
            _campanaExecutionService = campanaExecutionService ?? throw new ArgumentNullException(nameof(campanaExecutionService));
            _exportConfig = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        [Authorize]
        [HttpGet("ObtenerCampanaExecutions")]
        public async Task<IActionResult> GetAllAsync()
        {
            try {
                _logger.LogInformation("Obteniendo todas las ejecuciones de las campañas."); 

                var campanaExecutions = await _campanaExecutionService.GetAllAsync();
                if (campanaExecutions == null || !campanaExecutions.Any()) return NoContent();  

                return Ok(campanaExecutions);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error obteniendo todas las ejecuciones de las campañas.");
                return StatusCode(StatusCodes.Status500InternalServerError, 
                                  new { message = "Se produjo un error al obtener las ejecuciones de las campañas." });
            } 
        }

        [Authorize]
        [HttpGet("FiltrarCampanaExecutions")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] IFilters<CampanaExecution> filters,
                                                           [FromQuery] int? page,
                                                           [FromQuery] int? pageSize,
                                                           [FromQuery] string? orderBy,
                                                           [FromQuery] bool descending = false) { 
            var _filters = filters as CampanaExecutionFilters;

            if (_filters is null)
                throw new InvalidOperationException("El filtro recibido no es 'CampanaExecutionFilters'.");

            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var filtered = await _campanaExecutionService.GetByFiltersAsync(_filters, queryOptions);
            return Ok(filtered);
        }
         
        [Authorize]
        [HttpPost("CrearCampanaExecution")]
        public async Task<IActionResult> AddAsync([FromBody] CampanaExecution campanaExecution)
        {
            try {
                _logger.LogInformation("Creando una nueva ejecución de campaña.");

                var result = await _campanaExecutionService.AddAsync(campanaExecution);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("CampanaExecution:Crear", "Success"));
                    return Ok(result);
                }     
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error creando una nueva ejecución de campaña.");
                
            }
            return NoContent();
        }

        [Authorize]
        [HttpPut("ActualizarCampanaExecution")]
        public async Task<IActionResult> UpdateAsync([FromBody] CampanaExecution campanaExecution)
        {
            try {
                _logger.LogInformation("Actualizando una ejecución de campaña.");

                var result = await _campanaExecutionService.UpdateAsync(campanaExecution);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("CampanaExecution:Actualizar", "Success"));
                    return Ok(result);
                } 
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error actualizando una ejecución de campana.");
               
            }
            return NoContent();
        }

        [Authorize]
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(Guid id)
        {
            try {
                _logger.LogInformation("Eliminando una ejecución de campaña por Id.");

                var result = await _campanaExecutionService.Remove(id);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("CampanaExecution:Eliminar", "Success"));
                    return Ok(result);
                }  
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error eliminando una ejecución de campana por Id.");
              
            }
            return NoContent(); 
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
            var entityName = nameof(CampanaExecution);

            var file = await _campanaExecutionService.ExportarAsync(formato);

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