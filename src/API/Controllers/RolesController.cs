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
    public class RolesController : BaseController<Rol>, IController<IActionResult, Rol, Guid>
    { 
        private readonly IRolService _rolService; 

        public RolesController(ILogger<RolesController> logger, 
                               IRolService rolService) {
            _logger = logger; 
            _rolService = rolService ?? throw new ArgumentNullException(nameof(rolService));  
        }

        [Authorize]
        [HttpGet("ObtenerRoles")]
        public async Task<IActionResult> GetAllAsync()
        {
            var roles = await _rolService.GetAllAsync();
            return (roles != null && roles.Any()) ? Ok(roles) : NoContent();
        }

        [Authorize]
        [HttpGet("FiltrarRoles")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] IFilters<Rol>  filters,
                                                           [FromQuery] int? page,
                                                           [FromQuery] int? pageSize,
                                                           [FromQuery] string? orderBy,
                                                           [FromQuery] bool descending = false) {
            var _filters = filters as RolFilters;
            if (_filters is null)
                throw new InvalidOperationException("El filtro recibido no es 'RolFilters'.");

            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var filtered = await _rolService.GetByFiltersAsync(_filters, queryOptions);
            return Ok(filtered);
        }

        //[Authorize]
        //[HttpGet("ObtenerRol/{id}")]
        //public async Task<IActionResult> GetByIdAsync(Guid id)
        //{
        //    var role = await _rolService.GetByIdAsync(id); 
        //    return role != null ? Ok(role) : NoContent();
        //}

        [Authorize]
        [HttpPost("CrearRol")]
        public async Task<IActionResult> AddAsync([FromBody] Rol rol)
        { 
            var nuevoRol = new Rol {
                id = new Guid(),
                nombre = rol.nombre,
                descripcion = rol.descripcion
            };
             
            var result = await _rolService.AddAsync(nuevoRol);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("Rol:Crear", "Success"));
                return Ok(result);
            }
        }

        [Authorize]
        [HttpPut("ActualizarRol")]
        public async Task<IActionResult> UpdateAsync([FromBody] Rol rol)
        { 
            var result = await _rolService.UpdateAsync(rol);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("Rol:Actualizar", "Success"));
                return Ok(result);
            }  
        }

        [Authorize]
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(Guid id)
        { 
            try {
                var result = await _rolService.Remove(id);
                if (result == false) return NotFound();
                else
                {
                    _logger.LogInformation(MessageProvider.GetMessage("Rol:Eliminar", "Success"));
                    return Ok(result);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error eliminando el rol, {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("Rol:Eliminar", "Error"), id });
            }  
        }
    }
}