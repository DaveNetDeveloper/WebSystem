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
    [ApiController]
    [Route("[controller]")]
    public class WorkerServiceExecutionController : BaseController<WorkerServiceExecution>, 
                                                    IController<IActionResult, WorkerServiceExecution, Guid>
    { 
        private readonly IWorkerServiceExecutionService _workerServiceExecution;
        private readonly ExportConfiguration _exportConfig;

        public WorkerServiceExecutionController(ILogger<WorkerServiceExecutionController> logger, 
                                                IWorkerServiceExecutionService workerServiceExecution,
                                                IOptions<ExportConfiguration> options)
        {
            _logger = logger;
            _workerServiceExecution = workerServiceExecution ?? throw new ArgumentNullException(nameof(workerServiceExecution));
            _exportConfig = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        //[Authorize]
        [HttpGet("ObtenerWorkerServiceExecutions")]
        public async Task<IActionResult> GetAllAsync()
        {
            var workerServiceExecutions = await _workerServiceExecution.GetAllAsync();
            return (workerServiceExecutions != null && workerServiceExecutions.Any()) ? Ok(workerServiceExecutions) : NoContent();
        }

        [Authorize]
        [HttpGet("ObtenerWorkerServiceExecution/{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var workerServiceExecution = await _workerServiceExecution.GetByIdAsync(id);
            return workerServiceExecution != null ? Ok(workerServiceExecution) : NoContent();
        }

        [Authorize]
        [HttpPost("CrearWorkerServiceExecution")]
        public async Task<IActionResult> AddAsync([FromBody] WorkerServiceExecution workerServiceExecution)
        { 
            var nuevoWorkerServiceExecution = new WorkerServiceExecution {
                id = Guid.NewGuid(),
                workerService = workerServiceExecution.workerService,
                result = workerServiceExecution.result,
                resultDetailed = workerServiceExecution.resultDetailed,
                executionTime = workerServiceExecution.executionTime
            };
             
            var result = await _workerServiceExecution.AddAsync(nuevoWorkerServiceExecution);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("WorkerServiceExecution:Crear", "Success"));
                return Ok(result);
            }
        }

        [Authorize]
        [HttpPut("ActualizarWorkerServiceExecution")]
        public async Task<IActionResult> UpdateAsync([FromBody] WorkerServiceExecution workerServiceExecution)
        { 
            var result = await _workerServiceExecution.UpdateAsync(workerServiceExecution);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("WorkerServiceExecution:Actualizar", "Success"));
                return Ok(result);
            }  
        }

        [Authorize]
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(Guid id)
        { 
            try {
                var result = await _workerServiceExecution.Remove(id);
                if (result == false) return NotFound();
                else
                {
                    _logger.LogInformation(MessageProvider.GetMessage("WorkerServiceExecution:Eliminar", "Success"));
                    return Ok(result);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error eliminando el WorkerServiceExecution, {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("WorkerServiceExecution:Eliminar", "Error"), id });
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
            var entityName = nameof(WorkerServiceExecution);

            var file = await _workerServiceExecution.ExportarAsync(formato);

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

                //correoService.EnviarCorreo_Nuevo(correoN);

                //var tiposEnvioCorreo = await correoService.ObtenerTiposEnvioCorreo();
                //var tipoEnvioCorreo = tiposEnvioCorreo.Where(u => u.nombre == TipoEnvioCorreo.TipoEnvio.EnvioReport)
                //                                      .SingleOrDefault();

                //tipoEnvioCorreo.asunto = $"Report {entityName.ToString()} ({fileExtension})";
                //tipoEnvioCorreo.cuerpo = $"Se adjunta el informe para la vista de datos {entityName.ToString()}";

                //var correo = new Correo(tipoEnvioCorreo, _exportConfig.CorreoAdmin, "Admin", "");
                //correo.FicheroAdjunto = new FicheroAdjunto()
                //{
                //    Archivo = file,
                //    ContentType = contentType,
                //    NombreArchivo = fileName
                //};
                //correoService.EnviarCorreo(correo);
            }
            return File(file, contentType, fileName);
        }
    }
}