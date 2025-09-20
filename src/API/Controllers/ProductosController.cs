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
    public class ProductosController : BaseController<Producto>, IController<IActionResult, Producto, int>
    {  
        private readonly IProductoService _productoService;

        public ProductosController(ILogger<ProductosController> logger, IProductoService productoService) {
            _logger = logger; 
            _productoService = productoService ?? throw new ArgumentNullException(nameof(productoService));
        }

        [Authorize]
        [HttpGet("ObtenerProductos")]
        public async Task<IActionResult> GetAllAsync()
        {
            try {
                _logger.LogInformation("Obteniendo todos los productos."); 

                var productos = await _productoService.GetAllAsync();
                if (productos == null || !productos.Any()) return NoContent();  

                return Ok(productos);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error obteniendo todos los productos.");
                return StatusCode(StatusCodes.Status500InternalServerError, 
                                  new { message = "Se produjo un error al obtener los productos." });
            } 
        }

        [Authorize]
        [HttpGet("FiltrarProductos")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] IFilters<Producto> filters,
                                                           [FromQuery] int? page,
                                                           [FromQuery] int? pageSize,
                                                           [FromQuery] string? orderBy,
                                                           [FromQuery] bool descending = false) { 
            var _filters = filters as ProductoFilters;

            if (_filters is null)
                throw new InvalidOperationException("El filtro recibido no es 'ProductoFilters'.");

            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var filtered = await _productoService.GetByFiltersAsync(_filters, queryOptions);
            return Ok(filtered);
        }

        //[Authorize]
        //[HttpGet("ObtenerProducto/{id}")]
        //public async Task<IActionResult> GetByIdAsync(int id)
        //{
        //    try
        //    {
        //        _logger.LogInformation("Obteniendo un producto por Id.");

        //        var producto = await _productoService.GetByIdAsync(id);
        //        if (producto == null) return NoContent();

        //        return Ok(producto);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error obteniendo un producto por Id, {id}.", id);
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //                         new { message = "Se produjo un error al obtener un producto por el Id.", id });
        //    }
        //}

        [Authorize]
        [HttpPost("CrearProducto")]
        public async Task<IActionResult> AddAsync([FromBody] Producto producto)
        {
            try {
                _logger.LogInformation("Creando un nuevo producto.");

                var result = await _productoService.AddAsync(producto);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("Producto:Crear", "Success"));
                    return Ok(result);
                }     
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error creando un nuevo producto.");
                
            }
            return NoContent();
        }

        [Authorize]
        [HttpPut("ActualizarProducto")]
        public async Task<IActionResult> UpdateAsync([FromBody] Producto producto)
        {
            try {
                _logger.LogInformation("Actualizando un producto.");

                var result = await _productoService.UpdateAsync(producto);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("Producto:Actualizar", "Success"));
                    return Ok(result);
                } 
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error actualizando un producto.");
               
            }
            return NoContent();
        }

        [Authorize]
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            try {
                _logger.LogInformation("Eliminando un producto por Id.");

                var result = await _productoService.Remove(id);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("Producto:Eliminar", "Success"));
                    return Ok(result);
                }  
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error eliminando un producto por Id.");
              
            }
            return NoContent(); 
        }

        [Authorize]
        [HttpGet("GetImagenesByProducto/{id}")]
        public async Task<IActionResult> GetImagenesByProducto(int id)
        {
            try { 
                var imagenesByProduct = await _productoService.GetImagenesByProducto(id);

                if (imagenesByProduct == null || !imagenesByProduct.Any()) return NotFound();
                else {
                    //_logger.LogInformation(MessageProvider.GetMessage("Producto:GetImagenes", "Success"));
                    return Ok(imagenesByProduct);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error obteniendo las imagenes del producto {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("Producto:GetImagenes", "Error"), id });
            }
        } 

    }
}