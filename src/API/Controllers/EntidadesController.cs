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

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EntidadesController : BaseController<Entidad>, IController<IActionResult, Entidad, int>
    { 
        private readonly IEntidadService _entidadService;

        public EntidadesController(ILogger<EntidadesController> logger, IEntidadService entidadService)
        {
            _logger = logger; 
            _entidadService = entidadService ?? throw new ArgumentNullException(nameof(entidadService));
        }

        //[Authorize]
        [HttpGet("ObtenerEntidades")]
        public async Task<IActionResult> GetAllAsync()
        {
            var entidades = await _entidadService.GetAllAsync();
            return (entidades != null && entidades.Any()) ? Ok(entidades) : NoContent();
        }

        [Authorize]
        [HttpGet("FiltrarEntidades")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] IFilters<Entidad> filters,
                                                           [FromQuery] int? page,
                                                           [FromQuery] int? pageSize,
                                                           [FromQuery] string? orderBy,
                                                           [FromQuery] bool descending = false) {
            var _filters = filters as EntidadFilters;

            if (_filters is null)
                throw new InvalidOperationException("El filtro recibido no es 'EntidadFilters'.");

            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var filtered = await _entidadService.GetByFiltersAsync(_filters, queryOptions);
            return Ok(filtered);
        }

        //[Authorize]
        //[HttpGet("ObtenerEntidad/{id}")]
        //public async Task<IActionResult> GetByIdAsync(int id)
        //{ 
        //    var entidad = await _entidadService.GetByIdAsync(id); 
        //    return entidad != null ? Ok(entidad) : NoContent();  
        //}

        [Authorize]
        [HttpPost("CrearEntidad")]
        public async Task<IActionResult> AddAsync([FromBody] Entidad entidad)
        {
            var result = await _entidadService.AddAsync(entidad);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("Entidad:Crear", "Success"));
                return Ok(result);
            }   
        }

        [Authorize]
        [HttpPut("ActualizarEntidad")]
        public async Task<IActionResult> UpdateAsync([FromBody] Entidad entidad)
        {
            var result = await _entidadService.UpdateAsync(entidad);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("Entidad:Actualizar", "Success"));
                return Ok(result);
            }  
        }

        [Authorize]
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            try {
                var result = await _entidadService.Remove(id);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("Entidad:Eliminar", "Success"));
                    return Ok(result);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error eliminando la entidad, {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("Entidad:Eliminar", "Error"), id });
            } 
        }

        //
        // //Bindings - Table relations
        //

        [Authorize]
        [HttpGet("GetCategoriasByEntidad/{id}")]  
        public async Task<IActionResult> GetCategoriasByEntidad(int id)
        {
            try {  
                var categorias = await _entidadService.GetCategoriasByEntidad(id);

                if (categorias == null || !categorias.Any()) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("Entidad:GetCategorias", "Success"));
                    return Ok(categorias);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error obteniendo las categorias de la entidad {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("Entidad:GetCategorias", "Error"), id });
            } 
        } 
    } 
}