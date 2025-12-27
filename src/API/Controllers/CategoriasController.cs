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
    public class CategoriasController : BaseController<Categoria>, IController<IActionResult, Categoria, Guid>
    {
        private readonly ICategoriaService _categoriaService;
        private readonly ExportConfiguration _exportConfig;

        public CategoriasController(ILogger<CategoriasController> logger, 
                                    ICategoriaService categoriaService, 
                                    IOptions<ExportConfiguration> options)
        {
            _logger = logger;
            _categoriaService = categoriaService ?? throw new ArgumentNullException(nameof(categoriaService));
            _exportConfig = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        [AllowAnonymous]
        //[Authorize]
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
            var entityName = nameof(Categoria);

            var file = await _categoriaService.ExportarAsync(formato);

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