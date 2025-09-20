using Application.Common;
using Application.DTOs.Filters;
using Application.Interfaces.Controllers;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Services;
using Domain.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkerServiceExecutionController : BaseController<WorkerServiceExecution>, IController<IActionResult, WorkerServiceExecution, Guid>
    { 
        private readonly IWorkerServiceExecutionService _workerServiceExecution; 

        public WorkerServiceExecutionController(ILogger<WorkerServiceExecutionController> logger, IWorkerServiceExecutionService workerServiceExecution) {
            _logger = logger;
            _workerServiceExecution = workerServiceExecution ?? throw new ArgumentNullException(nameof(workerServiceExecution));  
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
                id = new Guid(),
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
    }
}