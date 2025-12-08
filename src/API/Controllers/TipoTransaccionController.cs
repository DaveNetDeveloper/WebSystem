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
using static Domain.Entities.Transaccion;
using static Utilities.ExporterHelper;
namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TipoTransaccionController : BaseController<TipoTransaccion>, 
                                             IController<IActionResult, TipoTransaccion, Guid>
    { 
        private readonly ITipoTransaccionService _tipoTransaccionService;
        private readonly ExportConfiguration _exportConfig;

        public TipoTransaccionController(ILogger<TipoTransaccionController> logger,
                                         ITipoTransaccionService tipoTransaccionService,
                                         IOptions<ExportConfiguration> options)
        {
            _logger = logger;
            _tipoTransaccionService = tipoTransaccionService ?? throw new ArgumentNullException(nameof(tipoTransaccionService));
            _exportConfig = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        [AllowAnonymous]
        //[Authorize]
        [HttpGet("ObtenerTipoTransacciones")]
        public async Task<IActionResult> GetAllAsync()
        {
            var tipoTransacciones = await _tipoTransaccionService.GetAllAsync();
            return (tipoTransacciones != null && tipoTransacciones.Any()) ? Ok(tipoTransacciones) : NoContent();
        }

        [Authorize]
        [HttpGet("FiltrarTipoTransacciones")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] IFilters<TipoTransaccion> filters,
                                                           [FromQuery] int? page,
                                                           [FromQuery] int? pageSize,
                                                           [FromQuery] string? orderBy,
                                                           [FromQuery] bool descending = false) {
            var _filters = filters as TipoTransaccionFilters;

            if (_filters is null)
                throw new InvalidOperationException("El filtro recibido no es 'TipoTransaccionFilters'.");

            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var filtered = await _tipoTransaccionService.GetByFiltersAsync(_filters, queryOptions);
            return Ok(filtered);
        }

        [Authorize]
        [HttpGet("ObtenerTipoTransaccion/{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var tipoTransaccion = await _tipoTransaccionService.GetByIdAsync(id);
            return tipoTransaccion != null ? Ok(tipoTransaccion) : NoContent();
        }

        [Authorize]
        [HttpPost("CrearTipoTransaccion")]
        public async Task<IActionResult> AddAsync([FromBody] TipoTransaccion tipoTransaccion)
        {
            var result = await _tipoTransaccionService.AddAsync(tipoTransaccion);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("TipoTransaccion:Crear", "Success"));
                return Ok(result);
            }   
        }

        [Authorize]
        [HttpPut("ActualizarTipoTransaccion")]
        public async Task<IActionResult> UpdateAsync([FromBody] TipoTransaccion tipoTransaccion)
        {
            var result = await _tipoTransaccionService.UpdateAsync(tipoTransaccion);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("TipoTransaccion:Actualizar", "Success"));
                return Ok(result);
            }  
        }

        [Authorize]
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(Guid id)
        {
            try {
                var result = await _tipoTransaccionService.Remove(id);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("TipoTransaccion:Eliminar", "Success"));
                    return Ok(result);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error eliminando el tipo de transacción, {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("TipoTransaccion:Eliminar", "Error"), id });
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
            var entityName = nameof(TipoTransaccion);

            var file = await _tipoTransaccionService.ExportarAsync(formato);
            
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