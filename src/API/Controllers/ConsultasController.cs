using Application.Common;
using Application.DTOs.Filters;
using Application.Interfaces.Controllers;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Services;
using Application.Services;
using Domain.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
 
namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ConsultasController : BaseController<Consulta>, IController<IActionResult, Consulta, Guid>
    {
        private readonly IConsultaService _consultaService;
        public ConsultasController(ILogger<ConsultasController> logger, 
                                   IConsultaService consultaService) {
            _logger = logger;
            _consultaService = consultaService ?? throw new ArgumentNullException(nameof(consultaService));
        }

        [Authorize]
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
                                                           [FromQuery] bool descending = false) {
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

        [Authorize]
        [HttpPost("CrearConsulta")]
        public async Task<IActionResult> AddAsync([FromBody] Consulta consulta)
        {
            var nuevaConsulta = new Consulta {
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
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("Consulta:Crear", "Success"));
                return Ok(result);
            }
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
    }
}