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
    public class MotivosConsultaController : BaseController<MotivoConsulta>, IController<IActionResult, MotivoConsulta, Guid>
    {
        private readonly IMotivoConsultaService _motivoConsultaService;
        public MotivosConsultaController(ILogger<MotivosConsultaController> logger, 
                                         IMotivoConsultaService motivoConsultaService) {
            _logger = logger;
            _motivoConsultaService = motivoConsultaService ?? throw new ArgumentNullException(nameof(motivoConsultaService));
        }

        [Authorize]
        [HttpGet("ObtenerMotivosConsulta")]
        public async Task<IActionResult> GetAllAsync()
        {
            var motivosConsulta = await _motivoConsultaService.GetAllAsync();
            return (motivosConsulta != null && motivosConsulta.Any()) ? Ok(motivosConsulta) : NoContent();
        }

        [Authorize]
        [HttpGet("FiltrarMotivoConsultas")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] IFilters<MotivoConsulta> filters,
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
    }
}