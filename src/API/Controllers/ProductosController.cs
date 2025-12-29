using Application.Common;
using Application.DTOs.Filters;
using Application.Interfaces.Controllers;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Services;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using static Domain.Entities.TipoEnvioCorreo;
using static Utilities.ExporterHelper;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductosController : BaseController<Producto>, 
                                       IController<IActionResult, Producto, int>
    {  
        private readonly IProductoService _productoService;
        private readonly ExportConfiguration _exportConfig;

        public ProductosController(ILogger<ProductosController> logger,
                                   IProductoService productoService,
                                   IOptions<ExportConfiguration> options)
        {
            _logger = logger; 
            _productoService = productoService ?? throw new ArgumentNullException(nameof(productoService));
            _exportConfig = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        [AllowAnonymous]
        //[Authorize]
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

        //[Authorize]
        [AllowAnonymous]
        [HttpGet("FiltrarProductos")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] ProductoFilters filters,
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
        [AllowAnonymous]
        [HttpGet("ObtenerProducto/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Obteniendo un producto por Id.");

                var producto = await _productoService.GetByIdAsync(id);
                if (producto == null) return NoContent();

                return Ok(producto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo un producto por Id, {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = "Se produjo un error al obtener un producto por el Id.", id });
            }
        }

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
        /// <summary>
        /// Exportar listado a Excel o pdf
        /// </summary> 
        /// <returns> File to download </returns>
        [HttpGet("Exportar")]
        //[Authorize(Policy = "RequireAdmin")]
        //[Authorize]
        [AllowAnonymous]
        public async Task<IActionResult> Exportar([FromServices] ICorreoService correoService,
                                                  [FromQuery] ExportFormat formato,
                                                  [FromQuery] bool envioEmail)
        {
            var entityName = nameof(Producto);
            var file = await _productoService.ExportarAsync(formato);

            string fileExtension = string.Empty;
            string contentType = string.Empty;

            switch (formato)
            {
                case ExportFormat.Excel:
                    contentType = _exportConfig.ExcelContentType;
                    fileExtension = _exportConfig.ExcelExtension;
                    break;
                case ExportFormat.Pdf:
                    contentType = _exportConfig.PdfContentType;
                    fileExtension = _exportConfig.PdfExtension;
                    break;
            }

            var fileName = $"List_{entityName.ToString()}_{DateTime.UtcNow:yyyyMMddHHmmss}{fileExtension}";

            if (envioEmail)
            {
                var tipoEnvio = await correoService.ObtenerTipoEnvioCorreo(TipoEnvioCorreos.EnvioReport);

                var context = new EnvioReportEmailContext(email: _exportConfig.CorreoAdmin,
                                                          nombre: "Admin",
                                                          nombreEntidad: "",
                                                          nombreInforme: $"List_{entityName.ToString()}");
                var correoN = new CorreoN
                {
                    Destinatario = context.Email,
                    Asunto = tipoEnvio.asunto,
                    Cuerpo = tipoEnvio.cuerpo
                };

                correoN.ApplyTags(context.GetTags());

                correoN.FicheroAdjunto = new FicheroAdjunto()
                {
                    Archivo = file,
                    ContentType = contentType,
                    NombreArchivo = fileName
                };
                correoService.EnviarCorreo_Nuevo(correoN);
            }
            return File(file, contentType, fileName);
        }

    }
}