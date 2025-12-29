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
using static Domain.Entities.TipoEnvioCorreo;
using static Utilities.ExporterHelper;
namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MotivosConsultaController : BaseController<MotivoConsulta>, IController<IActionResult, MotivoConsulta, Guid>
    {
        private readonly IMotivoConsultaService _motivoConsultaService;
        private readonly ExportConfiguration _exportConfig;
        public MotivosConsultaController(ILogger<MotivosConsultaController> logger, 
                                         IMotivoConsultaService motivoConsultaService,
                                         IOptions<ExportConfiguration> options)
        {
            _logger = logger;
            _motivoConsultaService = motivoConsultaService ?? throw new ArgumentNullException(nameof(motivoConsultaService));
            _exportConfig = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        [AllowAnonymous]
        //[Authorize]
        [HttpGet("ObtenerMotivosConsulta")]
        public async Task<IActionResult> GetAllAsync()
        {
            var motivosConsulta = await _motivoConsultaService.GetAllAsync();
            return (motivosConsulta != null && motivosConsulta.Any()) ? Ok(motivosConsulta) : NoContent();
        }

        //[Authorize]
        [AllowAnonymous]
        [HttpGet("FiltrarMotivoConsultas")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] MotivoConsultaFilters filters,
                                                           [FromQuery] int? page,
                                                           [FromQuery] int? pageSize,
                                                           [FromQuery] string? orderBy,
                                                           [FromQuery] bool descending = false) {
            var _filters = filters as MotivoConsultaFilters;

            if (_filters is null)
                throw new InvalidOperationException("El filtro recibido no es 'MotivoConsultaFilters'.");

            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var filtered = await _motivoConsultaService.GetByFiltersAsync(_filters, queryOptions);
            return Ok(filtered);
        }

        //[Authorize]
        //[HttpGet("ObtenerMotivoConsulta/{id}")]
        //public async Task<IActionResult> GetByIdAsync(Guid id)
        //{
        //    var motivoConsulta = await _motivoConsultaService.GetByIdAsync(id);
        //    return motivoConsulta != null ? Ok(motivoConsulta) : NoContent();
        //}

        [Authorize]
        [HttpPost("CrearMotivoConsulta")]
        public async Task<IActionResult> AddAsync([FromBody] MotivoConsulta motivoConsulta)
        {
            var nuevaMotivoConsulta = new MotivoConsulta { 
                nombre = motivoConsulta.nombre,
                descripcion = motivoConsulta.descripcion,
                idtipoentidad = motivoConsulta.idtipoentidad
            };

            var result = await _motivoConsultaService.AddAsync(nuevaMotivoConsulta);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("MotivoConsulta:Crear", "Success"));
                return Ok(result);
            }
        }

        [Authorize]
        [HttpPut("ActualizarMotivoConsulta")]
        public async Task<IActionResult> UpdateAsync([FromBody] MotivoConsulta motivoConsulta)
        {
            var result = await _motivoConsultaService.UpdateAsync(motivoConsulta);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("MotivoConsulta:Actualizar", "Success"));
                return Ok(result);
            }
        }

        [Authorize]
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(Guid id)
        {
            try {
                var result = await _motivoConsultaService.Remove(id);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("MotivoConsulta:Eliminar", "Success"));
                    return Ok(result);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error eliminando el motivo de consulta, {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("MotivoConsulta:Eliminar", "Error"), id });
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
            var entityName = nameof(MotivoConsulta);

            var file = await _motivoConsultaService.ExportarAsync(formato);

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

                correoN.FicheroAdjunto = new FicheroAdjunto(){
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