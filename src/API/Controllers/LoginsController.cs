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
using UAParser;
using static Domain.Entities.TipoEnvioCorreo;
using static Utilities.ExporterHelper;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginsController : BaseController<Login>, IController<IActionResult, Login, Guid>
    { 
        private readonly ILoginService _loginService;
        private readonly ExportConfiguration _exportConfig;

        public LoginsController(ILogger<LoginsController> logger, 
                                ILoginService loginService,
                                IOptions<ExportConfiguration> options)
        {
            _logger = logger;
            _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
            _exportConfig = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        [AllowAnonymous]
        //[Authorize]
        [HttpGet("ObtenerLogins")]
        public async Task<IActionResult> GetAllAsync()
        {
            var logins = await _loginService.GetAllAsync();
            return (logins != null && logins.Any()) ? Ok(logins) : NoContent();
        }

        [Authorize]
        [HttpGet("FiltrarLogins")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] IFilters<Login>  filters,
                                                           [FromQuery] int? page,
                                                           [FromQuery] int? pageSize,
                                                           [FromQuery] string? orderBy,
                                                           [FromQuery] bool descending = false) {
            var _filters = filters as LoginFilters;
            if (_filters is null)
                throw new InvalidOperationException("El filtro recibido no es 'LoginFilters'.");

            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var filtered = await _loginService.GetByFiltersAsync(_filters, queryOptions);
            return Ok(filtered);
        }

        [Authorize]
        [HttpGet("ObtenerLogin/{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var login = await _loginService.GetByIdAsync(id);
            return login != null ? Ok(login) : NoContent();
        }

        [Authorize]
        [HttpPost("CrearLogin")]
        public async Task<IActionResult> AddAsync([FromBody] Login login)
        { 
            var nuevoLogin = new Login {
                idUsuario = login.idUsuario,
                fecha = login.fecha,
                plataforma = login.plataforma,
                tipoDispositivo = device,
                modeloDispositivo = login.modeloDispositivo,
                sistemaOperativo = os,
                browser = browser.ToString(),
                ip = ip,
                pais = login.pais,
                region = login.region,
                idiomaNavegador = primaryLanguage
            };
             
            var result = await _loginService.AddAsync(nuevoLogin);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("Login:Crear", "Success"));
                return Ok(result);
            }
        }

        [Authorize]
        [HttpPut("ActualizarLogin")]
        public async Task<IActionResult> UpdateAsync([FromBody] Login login)
        { 
            var result = await _loginService.UpdateAsync(login);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("Login:Actualizar", "Success"));
                return Ok(result);
            }  
        }

        [Authorize]
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(Guid id)
        { 
            try {
                var result = await _loginService.Remove(id);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("Login:Eliminar", "Success"));
                    return Ok(result);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error eliminando el login, {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("Login:Eliminar", "Error"), id });
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
            var entityName = nameof(Login);

            var file = await _loginService.ExportarAsync(formato);

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