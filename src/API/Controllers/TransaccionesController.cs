using Application.Common;
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
using static Utilities.ExporterHelper;
namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransaccionesController : BaseController<Transaccion>, IController<IActionResult, Transaccion, int>
    {
        private readonly ITransaccionService _transaccionService;
        private readonly ExportConfiguration _exportConfig;

        public TransaccionesController(ILogger<TransaccionesController> logger, 
                                       ITransaccionService transaccionService,
                                       IOptions<ExportConfiguration> options)
        {
            _logger = logger; 
            _transaccionService = transaccionService ?? throw new ArgumentNullException(nameof(transaccionService));
            _exportConfig = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        [AllowAnonymous]
        //[Authorize]
        //[EnableRateLimiting("UsuariosLimiter")]
        [HttpGet("FiltrarTransacciones")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] IFilters<Transaccion> filters,
                                                           [FromQuery] int? page,
                                                           [FromQuery] int? pageSize,
                                                           [FromQuery] string? orderBy,
                                                           [FromQuery] bool descending = false) {
            var _filters = filters as TransaccionFilters;
            if (_filters is null)
                throw new InvalidOperationException("El filtro recibido no es 'TransaccionFilters'.");

            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var filteredTransactions = await _transaccionService.GetByFiltersAsync(_filters, queryOptions);
            return Ok(filteredTransactions);
        }

        [AllowAnonymous]
        //[Authorize]
        [HttpGet("ObtenerTransacciones")]
        public async Task<IActionResult> GetAllAsync()
        { 
            try {
                _logger.LogInformation("Obteniendo todas las transacciones.");

                var transacciones = await _transaccionService.GetAllAsync();
                return (transacciones != null && transacciones.Any() ? Ok(transacciones) : NoContent());
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error obteniendo todas las transacciones.");
                return NoContent();
            }  
        }
         
        [Authorize]
        [HttpPost("CrearTransaccion")]
        public async Task<IActionResult> AddAsync([FromBody] Transaccion transaccion)
        {
            var result = await _transaccionService.AddAsync(transaccion);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("Transaccion:Crear", "Success"));
                return Ok(result);
            } 
        }

        [Authorize]
        [HttpPut("ActualizarTransaccion")]
        public async Task<IActionResult> UpdateAsync([FromBody] Transaccion transaccion)
        { 
            var result = await _transaccionService.UpdateAsync(transaccion);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("Transaccion:Actualizar", "Success"));
                return Ok(result);
            } 
        }

        [Authorize]
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            try {
                var result = await _transaccionService.Remove(id);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("Transaccion:Eliminar", "Success"));
                    return Ok(result);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error eliminando la transacción, {id}.", id.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("Transaccion:Eliminar", "Error"), id });
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
            var entityName = nameof(Transaccion);

            var file = await _transaccionService.ExportarAsync(formato);

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