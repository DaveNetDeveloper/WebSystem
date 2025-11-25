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
    [ApiController]
    [Route("[controller]")]
    public class PerfilesController : BaseController<Perfil>, IController<IActionResult, Perfil, Guid>
    { 
        private readonly IPerfilService _perfilService; 

        public PerfilesController(ILogger<PerfilesController> logger,
                                IPerfilService perfilService) {
            _logger = logger;
            _perfilService = perfilService ?? throw new ArgumentNullException(nameof(perfilService));  
        }

        //[Authorize]
        [HttpGet("ObtenerPerfiles")]
        public async Task<IActionResult> GetAllAsync()
        {
            var perfiles = await _perfilService.GetAllAsync();
            return (perfiles != null && perfiles.Any()) ? Ok(perfiles) : NoContent();
        }

        [Authorize]
        [HttpGet("FiltrarPerfiles")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] IFilters<Perfil>  filters,
                                                           [FromQuery] int? page,
                                                           [FromQuery] int? pageSize,
                                                           [FromQuery] string? orderBy,
                                                           [FromQuery] bool descending = false) {
            var _filters = filters as PerfilFilters;
            if (_filters is null)
                throw new InvalidOperationException("El filtro recibido no es 'PerfilFilters'.");

            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var filtered = await _perfilService.GetByFiltersAsync(_filters, queryOptions);
            return Ok(filtered);
        }

        [Authorize]
        [HttpGet("ObtenerPerfil/{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var perfil = await _perfilService.GetByIdAsync(id);
            return perfil != null ? Ok(perfil) : NoContent();
        }

        [Authorize]
        [HttpPost("CrearPerfil")]
        public async Task<IActionResult> AddAsync([FromBody] Perfil perfil)
        {   
            var result = await _perfilService.AddAsync(perfil);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("Perfil:Crear", "Success"));
                return Ok(result);
            }
        }

        [Authorize]
        [HttpPut("ActualizarPerfil")]
        public async Task<IActionResult> UpdateAsync([FromBody] Perfil perfil)
        { 
            var result = await _perfilService.UpdateAsync(perfil);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("Perfil:Actualizar", "Success"));
                return Ok(result);
            }  
        }

        [Authorize]
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(Guid id)
        { 
            try {
                var result = await _perfilService.Remove(id);
                if (result == false) return NotFound();
                else
                {
                    _logger.LogInformation(MessageProvider.GetMessage("Perfil:Eliminar", "Success"));
                    return Ok(result);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error eliminando el perfil, {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("Perfil:Eliminar", "Error"), id });
            }  
        }
    }
}