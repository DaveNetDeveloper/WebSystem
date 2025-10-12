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
    public class RecompensasController : BaseController<Recompensa>, 
                                         IController<IActionResult, Recompensa, int>
    {
        private readonly IRecompensaService _recompensaService;
        public RecompensasController(ILogger<RecompensasController> logger, 
                                     IRecompensaService recompensaService) {
            _logger = logger;
            _recompensaService = recompensaService ?? throw new ArgumentNullException(nameof(recompensaService));
        }

        [Authorize]
        [HttpGet("ObtenerRecompensas")]
        public async Task<IActionResult> GetAllAsync()
        {
            var recompensas = await _recompensaService.GetAllAsync();
            return (recompensas != null && recompensas.Any()) ? Ok(recompensas) : NoContent();
        }

        [Authorize]
        [HttpGet("FiltrarRecompensas")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] IFilters<Recompensa>  filters,
                                                           [FromQuery] int? page,
                                                           [FromQuery] int? pageSize,
                                                           [FromQuery] string? orderBy,
                                                           [FromQuery] bool descending = false) {
            var _filters = filters as RecompensaFilters;
            if (_filters is null)
                throw new InvalidOperationException("El filtro recibido no es 'RecompensaFilters'.");

            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var filtered = await _recompensaService.GetByFiltersAsync(_filters, queryOptions);
            return Ok(filtered);
        }

        [Authorize]
        [HttpGet("ObtenerRecompensa/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var recompensa = await _recompensaService.GetByIdAsync(id);
            return recompensa != null ? Ok(recompensa) : NoContent();
        }


        [Authorize]
        [HttpPost("CrearRecompensa")]
        public async Task<IActionResult> AddAsync([FromBody] Recompensa recompensa)
        {
            var nuevaRecompensa = new Recompensa {
                id = -1,
                nombre = recompensa.nombre,
                descripcion = recompensa.descripcion,
                identidad = recompensa.identidad
            };

            var result = await _recompensaService.AddAsync(nuevaRecompensa);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("Recompensa:Crear", "Success"));
                return Ok(result);
            }
        }

        [Authorize]
        [HttpPut("ActualizarRecompensa")]
        public async Task<IActionResult> UpdateAsync([FromBody] Recompensa recompensa)
        {
            var result = await _recompensaService.UpdateAsync(recompensa);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("Recompensa:Actualizar", "Success"));
                return Ok(result);
            }
        }

        [Authorize]
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            try {
                var result = await _recompensaService.Remove(id);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("Recompensa:Eliminar", "Success"));
                    return Ok(result);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error eliminando la recompensa, {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("Recompensa:Eliminar", "Error"), id });
            }
        }

        //[Authorize]
        [HttpGet("GetRecompensasByUsuario/{idUsuario}")]
        public async Task<IActionResult> GetRecompensasByUsuario(int idUsuario)
        {
            try {
                var recompensasByUser = await _recompensaService.GetRecompensasByUsuario(idUsuario);

                if (recompensasByUser == null || !recompensasByUser.Any()) return NotFound();
                else {
                    //_logger.LogInformation(MessageProvider.GetMessage("Recompensa:GetByUser", "Success"));
                    return Ok(recompensasByUser);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error obteniendo las recompensas del usuario {id}.", idUsuario);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("Recompensa:GetByUser", "Error"), idUsuario });
            }
        }
    }
}