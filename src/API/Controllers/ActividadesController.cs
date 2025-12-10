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
    public class ActividadesController : BaseController<Actividad>, IController<IActionResult, Actividad, int>
    { 
        private readonly IActividadService _actividadService;
        private readonly ExportConfiguration _exportConfig;
        public ActividadesController(ILogger<ActividadesController> logger, 
                                     IActividadService actividadService,
                                     IOptions<ExportConfiguration> options) {
            _logger = logger;
            _actividadService = actividadService ?? throw new ArgumentNullException(nameof(actividadService));
            _exportConfig = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        [AllowAnonymous]
        //[Authorize]
        [HttpGet("ObtenerActividades")]
        public async Task<IActionResult> GetAllAsync()
        {
            var actividades = await _actividadService.GetAllAsync();
            return (actividades != null && actividades.Any()) ? Ok(actividades) : NoContent();
        }

        //[Authorize]
        [AllowAnonymous]
        [HttpGet("FiltrarActividades")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] ActividadFilters filters,
                                                           [FromQuery] int? page,
                                                           [FromQuery] int? pageSize,
                                                           [FromQuery] string? orderBy,
                                                           [FromQuery] bool descending = false) {
            var _filters = filters as ActividadFilters;

            if (_filters is null)
                throw new InvalidOperationException("El filtro recibido no es 'ActividadFilters'.");
            //else
                //_filters = ((ActividadFilters)(IFilters<Actividad>)filters);

            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var filtered = await _actividadService.GetByFiltersAsync(_filters, queryOptions);
            return Ok(filtered);
        }

        [Authorize]
        [HttpGet("ObtenerActividad/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var actividad = await _actividadService.GetByIdAsync(id);
            return actividad != null ? Ok(actividad) : NoContent(); 
        }

        [Authorize]
        [HttpPost("CrearActividad")]
        public async Task<IActionResult> AddAsync([FromBody] Actividad actividad)
        {
            var nuevaActividad = new Actividad {
                id = -1,
                idEntidad = actividad.idEntidad,
                nombre = actividad.nombre,
                descripcion = actividad.descripcion,
                linkEvento = actividad.linkEvento,
                idTipoActividad = actividad.idTipoActividad,
                ubicacion = actividad.ubicacion,
                popularidad = actividad.popularidad,
                descripcionCorta = actividad.descripcionCorta,
                fechaInicio = actividad.fechaInicio,
                fechaFin = actividad.fechaFin,
                gratis = actividad.gratis,
                activo = actividad.activo,
                informacioExtra = actividad.informacioExtra,
                linkInstagram = actividad.linkInstagram,
                linkFacebook = actividad.linkFacebook,
                linkYoutube = actividad.linkYoutube
            }; 

            var result = await _actividadService.AddAsync(nuevaActividad);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("Actividad:Crear", "Success"));
                return Ok(result);
            } 
        }

        [Authorize]
        [HttpPut("ActualizarActividad")]
        public async Task<IActionResult> UpdateAsync([FromBody] Actividad actividad)
        {
            var result = await _actividadService.UpdateAsync(actividad); 
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("Actividad:Actualizar", "Success"));
                return Ok(result);
            }
        }

        [Authorize]
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(int id)
        { 
            try {
                var result = await _actividadService.Remove(id);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("Actividad:Eliminar", "Success"));
                    return Ok(result);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error eliminando la actividad, {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("Actividad:Eliminar", "Error"), id });
            }  
        }
        
        //[Authorize]
        [HttpGet("ObtenerActividadesByTipoActividad/{id}")]
        public async Task<IActionResult> GetActividadesByTipoActividad(Guid id)
        {
            var actividades = await _actividadService.GetActividadesByTipoActividad(id);
            return actividades != null ? Ok(actividades) : NoContent();
        }

        //
        // //Bindings - Table relations
        //
        [Authorize]
        [HttpGet("GetImagenesByActividad/{id}")]
        public async Task<IActionResult> GetImagenesByActividad(int id)
        {
            try {
                var imagenesByActivity = await _actividadService.GetImagenesByActividad(id);

                if (imagenesByActivity == null || !imagenesByActivity.Any()) return NotFound();
                else {
                    //_logger.LogInformation(MessageProvider.GetMessage("Actividad:GetImagenes", "Success"));
                    return Ok(imagenesByActivity);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error obteniendo las imagenes de la actividad {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("Actividad:GetImagenes", "Error"), id });
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
            var entityName = nameof(Entidad);

            var file = await _actividadService.ExportarAsync(formato);

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