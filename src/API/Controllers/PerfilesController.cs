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
    public class PerfilesController : BaseController<Perfil>, IController<IActionResult, Perfil, Guid>
    { 
        private readonly IPerfilService _perfilService;
        private readonly ExportConfiguration _exportConfig;

        public PerfilesController(ILogger<PerfilesController> logger,
                                  IPerfilService perfilService,
                                  IOptions<ExportConfiguration> options)
        {
            _logger = logger;
            _perfilService = perfilService ?? throw new ArgumentNullException(nameof(perfilService));
            _exportConfig = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        [AllowAnonymous]
        //[Authorize]
        [HttpGet("ObtenerPerfiles")]
        public async Task<IActionResult> GetAllAsync()
        {
            var perfiles = await _perfilService.GetAllAsync();
            return (perfiles != null && perfiles.Any()) ? Ok(perfiles) : NoContent();
        }

        [Authorize]
        [HttpGet("FiltrarPerfiles")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] IFilters<Perfil>  filters,
                                                           [FromQuery] int? page,
                                                           [FromQuery] int? pageSize,
                                                           [FromQuery] string? orderBy,
                                                           [FromQuery] bool descending = false) {
            var _filters = filters as PerfilFilters;
            if (_filters is null)
                throw new InvalidOperationException("El filtro recibido no es 'PerfilFilters'.");

            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var filtered = await _perfilService.GetByFiltersAsync(_filters, queryOptions);
            return Ok(filtered);
        }

        [Authorize]
        [HttpGet("ObtenerPerfil/{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var perfil = await _perfilService.GetByIdAsync(id);
            return perfil != null ? Ok(perfil) : NoContent();
        }

        [Authorize]
        [HttpPost("CrearPerfil")]
        public async Task<IActionResult> AddAsync([FromBody] Perfil perfil)
        {   
            var result = await _perfilService.AddAsync(perfil);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("Perfil:Crear", "Success"));
                return Ok(result);
            }
        }

        [Authorize]
        [HttpPut("ActualizarPerfil")]
        public async Task<IActionResult> UpdateAsync([FromBody] Perfil perfil)
        { 
            var result = await _perfilService.UpdateAsync(perfil);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("Perfil:Actualizar", "Success"));
                return Ok(result);
            }  
        }

        [Authorize]
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(Guid id)
        { 
            try {
                var result = await _perfilService.Remove(id);
                if (result == false) return NotFound();
                else
                {
                    _logger.LogInformation(MessageProvider.GetMessage("Perfil:Eliminar", "Success"));
                    return Ok(result);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error eliminando el perfil, {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("Perfil:Eliminar", "Error"), id });
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
            var entityName = nameof(Perfil);

            var file = await _perfilService.ExportarAsync(formato);

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