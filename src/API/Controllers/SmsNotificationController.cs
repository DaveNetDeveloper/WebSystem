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
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class SmsNotificationController : BaseController<SmsNotification>, 
                                             IController<IActionResult, SmsNotification, Guid>
    { 
        private readonly ISmsNotificationService _smsNotificationService;
        private readonly ExportConfiguration _exportConfig;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="smsNotificationService"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public SmsNotificationController(ILogger<SmsNotificationController> logger,
                                         ISmsNotificationService smsNotificationService,
                                   IOptions<ExportConfiguration> options)   
        {
            _logger = logger;
            _smsNotificationService = smsNotificationService ?? throw new ArgumentNullException(nameof(smsNotificationService));
            _exportConfig = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary> Envia un SMS </summary>
        /// <param name="SmsDTO"> SMS con los datos del envio </param> 
        [Authorize]
        [HttpPost("Enviar")]
        public IActionResult EnviarSms([FromBody][Required] SmsDTO sms)
        {
            try {
                var result = _smsNotificationService.EnviarSms(sms);
                return Ok("SMS enviado correctamente");
            }
            catch (Exception ex) {
                return BadRequest($"Error al enviar el SMS: {ex.Message}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //[Authorize]
        [HttpGet("ObtenerSmsNotifications")]
        public async Task<IActionResult> GetAllAsync()
        {
            var smsNotification = await _smsNotificationService.GetAllAsync();
            return (smsNotification != null && smsNotification.Any()) ? Ok(smsNotification) : NoContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("ObtenerTiposEnvioSms")]
        public async Task<IActionResult> ObtenerTiposEnvioSms()
        {
            var tiposEnvioSms = await _smsNotificationService.ObtenerTiposEnvioSms();
            return (tiposEnvioSms != null && tiposEnvioSms.Any()) ? Ok(tiposEnvioSms) : NoContent();
        } 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="descending"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [Authorize]
        [HttpGet("FiltrarSmsNotifications")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] IFilters<SmsNotification> filters,
                                                           [FromQuery] int? page,
                                                           [FromQuery] int? pageSize,
                                                           [FromQuery] string? orderBy,
                                                           [FromQuery] bool descending = false) {
            var _filters = filters as SmsNotificationFilters;

            if (_filters is null)
                throw new InvalidOperationException("El filtro recibido no es 'SmsNotificationFilters'.");
            //else
            //_filters = ((SmsNotificationFilters)(IFilters<SmsNotification>)filters);

            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var filtered = await _smsNotificationService.GetByFiltersAsync(_filters, queryOptions);
            return Ok(filtered);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="smsNotification"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("CrearSmsNotification")]
        public async Task<IActionResult> AddAsync([FromBody] SmsNotification smsNotification)
        {
            var nuevaSmsNotification = new SmsNotification
            {
                id = smsNotification.id,
                idUsuario = smsNotification.idUsuario,
                tipoEnvioSms = smsNotification.tipoEnvioSms,
                fechaCreacion = DateTime.UtcNow,
                fechaEnvio = smsNotification.fechaEnvio,
                titulo = smsNotification.titulo,
                mensaje = smsNotification.mensaje,
                activo = smsNotification.activo,
                telefono = smsNotification.telefono
            }; 

            var result = await _smsNotificationService.AddAsync(nuevaSmsNotification);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("SmsNotification:Crear", "Success"));
                return Ok(result);
            } 
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="smsNotification"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("ActualizarSmsNotification")]
        public async Task<IActionResult> UpdateAsync([FromBody] SmsNotification smsNotification)
        {
            var result = await _smsNotificationService.UpdateAsync(smsNotification); 
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("SmsNotification:Actualizar", "Success"));
                return Ok(result);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(Guid id)
        { 
            try {
                var result = await _smsNotificationService.Remove(id);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("SmsNotification:Eliminar", "Success"));
                    return Ok(result);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error eliminando la SmsNotification, {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("SmsNotification:Eliminar", "Error"), id });
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
            var entityName = nameof(SmsNotification);

            var file = await _smsNotificationService.ExportarAsync(formato);

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