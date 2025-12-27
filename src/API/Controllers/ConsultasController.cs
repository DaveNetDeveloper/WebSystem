using Application.Common;
using Application.DTOs.Filters;
using Application.Interfaces.Controllers;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Services;
using Application.Services;
using DocumentFormat.OpenXml.ExtendedProperties;
using Domain.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using static Domain.Entities.TipoEnvioCorreo;
using static Utilities.ExporterHelper;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ConsultasController : BaseController<Consulta>//, IController<IActionResult, Consulta, Guid>
    {
        private readonly IConsultaService _consultaService;
        private readonly ExportConfiguration _exportConfig;
        public ConsultasController(ILogger<ConsultasController> logger,
                                   IConsultaService consultaService,
                                   IOptions<ExportConfiguration> options)
        {
            _logger = logger;
            _consultaService = consultaService ?? throw new ArgumentNullException(nameof(consultaService));
            _exportConfig = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        [AllowAnonymous]
        //[Authorize]
        [HttpGet("ObtenerConsultas")]
        public async Task<IActionResult> GetAllAsync()
        {
            var consultas = await _consultaService.GetAllAsync();
            return (consultas != null && consultas.Any()) ? Ok(consultas) : NoContent();
        }

        [Authorize]
        [HttpGet("FiltrarConsultas")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] IFilters<Consulta> filters,
                                                           [FromQuery] int? page,
                                                           [FromQuery] int? pageSize,
                                                           [FromQuery] string? orderBy,
                                                           [FromQuery] bool descending = false)
        {
            var _filters = filters as ConsultaFilters;

            if (_filters is null)
                throw new InvalidOperationException("El filtro recibido no es 'ConsultaFilters'.");

            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var filtered = await _consultaService.GetByFiltersAsync(_filters, queryOptions);
            return Ok(filtered);
        }

        //[Authorize]
        //[HttpGet("ObtenerConsulta/{id}")]
        //public async Task<IActionResult> GetByIdAsync(Guid id)
        //{
        //    var consulta = await _consultaService.GetByIdAsync(id);
        //    return consulta != null ? Ok(consulta) : NoContent();
        //}

        //[Authorize]
        [AllowAnonymous]
        [HttpPost("CrearConsulta")]
        public async Task<IActionResult> AddAsync([FromBody] Consulta consulta,
                                                  [FromServices] ICorreoService correoService,
                                                  [FromServices] IUsuarioService usuarioService,
                                                  [FromServices] IEntidadService entidadService)
        {
            var nuevaConsulta = new Consulta
            {
                nombreCompleto = consulta.nombreCompleto,
                email = consulta.email,
                telefono = consulta.telefono,
                asunto = consulta.asunto,
                mensaje = consulta.mensaje,
                fecha = consulta.fecha,
                idMotivoConsulta = consulta.idMotivoConsulta,
                idEntidad = consulta.idEntidad
            };

            var result = await _consultaService.AddAsync(nuevaConsulta);
            if (result == false) return NoContent();
            else
            {
                // Obtene info de la entidad
                var entidades = await entidadService.GetByFiltersAsync(new EntidadFilters { Activo = true, Id = consulta.idEntidad }, null);
                var entidad = entidades.SingleOrDefault();

                // Obtener usuario manager
                var usuarios = await usuarioService.GetByFiltersAsync(new UsuarioFilters { Activo = true, Correo = entidad.manager }, null);
                var manager = usuarios.SingleOrDefault();

                if (null != manager) 
                {
                     
                    var tipoEnvioCorreo = await correoService.ObtenerTipoEnvioCorreo(TipoEnvioCorreo.TipoEnvioCorreos.ConsultaUsuario_Manager);
                    
                    var context = new EnvioConsultaUsuarioManagerEmailContext(email: manager.correo,
                                                                              nombre: manager.nombre,
                                                                              nombreEntidad : entidad.nombre,
                                                                              nombreUsuario: consulta.nombreCompleto,
                                                                              correoUsuario: consulta.email);
                    var correoN = new CorreoN {
                        Destinatario = context.Email,
                        Asunto = tipoEnvioCorreo.asunto,
                        Cuerpo = tipoEnvioCorreo.cuerpo
                    };

                    correoN.ApplyTags(context.GetTags());
                    correoService.EnviarCorreo_Nuevo(correoN);
                }
            }
            //_logger.LogInformation(MessageProvider.GetMessage("Consulta:Crear", "Success"));
            return Ok(result);  
        }

        [Authorize]
        [HttpPut("ActualizarConsulta")]
        public async Task<IActionResult> UpdateAsync([FromBody] Consulta consulta)
        {
            var result = await _consultaService.UpdateAsync(consulta);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("Consulta:Actualizar", "Success"));
                return Ok(result);
            }
        }

        [Authorize]
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(Guid id)
        {
            try {
                var result = await _consultaService.Remove(id);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("Consulta:Eliminar", "Success"));
                    return Ok(result);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error eliminando la consulta, {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("Consulta:Eliminar", "Error"), id });
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
            var entityName = nameof(Consulta);

            var file = await _consultaService.ExportarAsync(formato);

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