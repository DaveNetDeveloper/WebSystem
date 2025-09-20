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
    public class CategoriasController : BaseController<Categoria>, IController<IActionResult, Categoria, Guid>
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriasController(ILogger<CategoriasController> logger, ICategoriaService categoriaService)
        {
            _logger = logger;
            _categoriaService = categoriaService ?? throw new ArgumentNullException(nameof(categoriaService));  
        }

        [Authorize]
        [HttpGet("ObtenerCategorias")]
        public async Task<IActionResult> GetAllAsync()
        {
            try {
                _logger.LogInformation("Obteniendo todas las categorias.");

                var categorias = await _categoriaService.GetAllAsync();
                return (categorias != null && categorias.Any() ? Ok(categorias) : NoContent());

                return Ok(categorias);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error obteniendo todos las categorias.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  new { message = "Se produjo un error al obtener las categorias." });
            }
        }

        [Authorize]
        [HttpGet("FiltrarCategorias")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] IFilters<Categoria> filters,
                                                           [FromQuery] int? page,
                                                           [FromQuery] int? pageSize,
                                                           [FromQuery] string? orderBy,
                                                           [FromQuery] bool descending = false) {
            var _filters = filters as CategoriaFilters;

            if (_filters is null)
                throw new InvalidOperationException("El filtro recibido no es 'CategoriaFilters'.");

            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var filtered = await _categoriaService.GetByFiltersAsync(_filters, queryOptions);
            return Ok(filtered);
        }

        //[Authorize]
        //[HttpGet("ObtenerCategoria/{id}")]
        //public async Task<IActionResult> GetByIdAsync(Guid id)
        //{
        //    try {
        //        _logger.LogInformation("Obteniendo una categoria por Id.");

        //        var categoria = await _categoriaService.GetByIdAsync(id);
        //        return categoria != null ? Ok(categoria) : NoContent();

        //        return Ok(categoria);
        //    }
        //    catch (Exception ex) {
        //        _logger.LogError(ex, "Error obteniendo una categoria por Id, {id}.", id);
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //                         new { message = "Se produjo un error al obtener una categoria por el Id.", id });
        //    }
        //}

        [Authorize]
        [HttpPost("CrearCategoria")]
        public async Task<IActionResult> AddAsync([FromBody] Categoria categoria)
        {
            var result = await _categoriaService.AddAsync(categoria);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("Categoria:Crear", "Success"));
                return Ok(result);
            }  
        }

        [Authorize]
        [HttpPut("ActualizarCategoria")]
        public async Task<IActionResult> UpdateAsync([FromBody] Categoria categoria)
        {
            var result = await _categoriaService.UpdateAsync(categoria);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("Categoria:Actualizar", "Success"));
                return Ok(result);
            }
        }

        [Authorize]
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(Guid id)
        {
            try {
                var result = await _categoriaService.Remove(id);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("Categoria:Eliminar", "Success"));
                    return Ok(result);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error eliminando la categoria, {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("Categoria:Eliminar", "Error"), id });
            } 
        }
    }
}