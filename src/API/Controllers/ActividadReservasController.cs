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
    public class ActividadReservasController : BaseController<ActividadReserva>, 
                                               IController<IActionResult, ActividadReserva, Guid>
    {
        private readonly IActividadReservaService _actividadReservaService;
        public ActividadReservasController(ILogger<ActividadReservasController> logger,
                                           IActividadReservaService actividadReservaService) {
            _logger = logger;
            _actividadReservaService = actividadReservaService ?? throw new ArgumentNullException(nameof(actividadReservaService));
        }

        [Authorize]
        [HttpGet("ObtenerActividadReservas")]
        public async Task<IActionResult> GetAllAsync()
        {
            var reservas = await _actividadReservaService.GetAllAsync();
            return (reservas != null && reservas.Any()) ? Ok(reservas) : NoContent();
        }

        [Authorize]
        [HttpGet("FiltrarActividadReservas")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] IFilters<ActividadReserva>  filters,
                                                           [FromQuery] int? page,
                                                           [FromQuery] int? pageSize,
                                                           [FromQuery] string? orderBy,
                                                           [FromQuery] bool descending = false) {
            var _filters = filters as ActividadReservaFilters;
            if (_filters is null)
                throw new InvalidOperationException("El filtro recibido no es ActividadReservaFilters'.");

            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var filtered = await _actividadReservaService.GetByFiltersAsync(_filters, queryOptions);
            return Ok(filtered);
        }

        [Authorize]
        [HttpGet("ObtenerActividadReserva/{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var reserva = await _actividadReservaService.GetByIdAsync(id);
            return reserva != null ? Ok(reserva) : NoContent();
        }


        [Authorize]
        [HttpPost("CrearActividadReserva")]
        public async Task<IActionResult> AddAsync([FromBody] ActividadReserva actividadReserva)
        {
            var nuevaReserva = new ActividadReserva
            { 
                idActividad = actividadReserva.idActividad,
                idUsuario = actividadReserva.idUsuario,
                codigoReserva = actividadReserva.codigoReserva,
                fechaReserva = actividadReserva.fechaReserva,
                fechaActividad = actividadReserva.fechaActividad,
                fechaValidacion = actividadReserva.fechaValidacion,
                estado = actividadReserva.estado
            };

            var result = await _actividadReservaService.AddAsync(nuevaReserva);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("ActividadReserva:Crear", "Success"));
                return Ok(result);
            }
        }

        [Authorize]
        [HttpPut("ActualizarActividadReserva")]
        public async Task<IActionResult> UpdateAsync([FromBody] ActividadReserva actividadReserva)
        {
            var result = await _actividadReservaService.UpdateAsync(actividadReserva);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("ActividadReserva:Actualizar", "Success"));
                return Ok(result);
            }
        }

        [Authorize]
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(Guid id)
        {
            try {
                var result = await _actividadReservaService.Remove(id);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("ActividadReserva:Eliminar", "Success"));
                    return Ok(result);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error eliminando la reserva, {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("ActividadReserva:Eliminar", "Error"), id });
            }
        }

        ////[Authorize]
        //[HttpGet("GetRecompensasByUsuario/{idUsuario}")]
        //public async Task<IActionResult> GetRecompensasByUsuario(int idUsuario)
        //{
        //    try {
        //        var recompensasByUser = await _actividadReservaService.GetRecompensasByUsuario(idUsuario);

        //        if (recompensasByUser == null || !recompensasByUser.Any()) return NotFound();
        //        else {
        //            //_logger.LogInformation(MessageProvider.GetMessage("Recompensa:GetByUser", "Success"));
        //            return Ok(recompensasByUser);
        //        }
        //    }
        //    catch (Exception ex) {
        //        _logger.LogError(ex, "Error obteniendo las recompensas del usuario {id}.", idUsuario);
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //                         new { message = MessageProvider.GetMessage("Recompensa:GetByUser", "Error"), idUsuario });
        //    }
        //}
    }
}