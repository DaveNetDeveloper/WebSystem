using Application.Common;
using Application.DTOs.Filters;
using Application.Interfaces.Controllers;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Services;
using Application.Services;
using Domain.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static Domain.Entities.Transaccion;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TipoRecompensasController : BaseController<TipoRecompensa>, 
                                             IController<IActionResult, TipoRecompensa, Guid>
    { 
        private readonly IRecompensaService _tipoRecompensaService;

        public TipoRecompensasController(ILogger<TipoRecompensasController> logger,
                                         IRecompensaService tipoTransaccionService)
        {
            _logger = logger;
            _tipoRecompensaService = tipoTransaccionService ?? throw new ArgumentNullException(nameof(tipoTransaccionService));
        }

        [Authorize]
        [HttpGet("ObtenerTipoRecompensas")]
        public async Task<IActionResult> GetAllAsync()
        {
            var tipoRecompensas = await _tipoRecompensaService.GetAllTiposRecompensas();
            return (tipoRecompensas != null && tipoRecompensas.Any()) ? Ok(tipoRecompensas) : NoContent();
        }

        [Authorize]
        [HttpGet("ObtenerTipoRecompensa/{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var tipoRecompensa = await _tipoRecompensaService.GetTipoRecompensa(id);
            return tipoRecompensa != null ? Ok(tipoRecompensa) : NoContent();
        }

        [Authorize]
        [HttpPost("CrearTipoRecompensa")]
        public async Task<IActionResult> AddAsync([FromBody] TipoRecompensa tipoRecompensa)
        {
            var result = await _tipoRecompensaService.AddTipoRecompensa(tipoRecompensa);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("TipoRecompensa:Crear", "Success"));
                return Ok(result);
            }   
        }

        [Authorize]
        [HttpPut("ActualizarTipoRecompensa")]
        public async Task<IActionResult> UpdateAsync([FromBody] TipoRecompensa tipoRecompensa)
        {
            var result = await _tipoRecompensaService.UpdateTipoRecompensa(tipoRecompensa);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("TipoRecompensa:Actualizar", "Success"));
                return Ok(result);
            }  
        }

        [Authorize]
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(Guid id)
        {
            try {
                var result = await _tipoRecompensaService.RemoveTipoRecompensa(id);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("TipoRecompensa:Eliminar", "Success"));
                    return Ok(result);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error eliminando el tipo de recompensa, {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("TipoRecompensa:Eliminar", "Error"), id });
            } 
        } 
    } 
}