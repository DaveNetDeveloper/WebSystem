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
using System.ComponentModel.DataAnnotations; 
using static Utilities.ExporterHelper;
using static Domain.Entities.TipoEnvioCorreo;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EntidadesController : BaseController<Entidad>, 
                                       IController<IActionResult, Entidad, int> 
    { 
        private readonly IEntidadService _entidadService;
        private readonly ExportConfiguration _exportConfig;
        public EntidadesController(ILogger<EntidadesController> logger,    
                                   IEntidadService entidadService,
                                   IOptions<ExportConfiguration> options)
        {
            _logger = logger; 
            _entidadService = entidadService ?? throw new ArgumentNullException(nameof(entidadService));
            _exportConfig = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        [AllowAnonymous]
        //[Authorize]
        ////[Authorize(Roles = "Admin,Manager")]
        [HttpGet("ObtenerEntidades")]
        public async Task<IActionResult> GetAllAsync()
        {
            var entidades = await _entidadService.GetAllAsync();

            // si existe la cookkie [entitiesIds] aplicar filtro
            var valor = Request.Cookies["Entidades-Cookie"];

            if(valor != null && !string.IsNullOrEmpty(valor.Trim()))
            {
                entidades = entidades.Where(e => valor.Split(',').Contains(e.id.ToString())).ToList();
            }

            return (entidades != null && entidades.Any()) ? Ok(entidades) : NoContent();
        }

        //[Authorize]
        [AllowAnonymous]
        [HttpGet("FiltrarEntidades")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] EntidadFilters filters,
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
        [HttpGet("ObtenerEntidad/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var entidad = await _entidadService.GetByIdAsync(id);
            return entidad != null ? Ok(entidad) : NoContent();
        }

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

        /// <summary>
        /// Exportar listado a Excel o pdf
        /// </summary> 
        /// <returns> File to download </returns>
        [HttpGet("Exportar")]
        //[Authorize(Policy = "RequireAdmin")]
        //[Authorize]
        [AllowAnonymous]
        public async Task<IActionResult> Exportar([FromServices] ICorreoService correoService,
                                                  [FromQuery]   ExportFormat formato,
                                                  [FromQuery]   bool envioEmail) {
            var entityName = nameof(Entidad);

            var file = await _entidadService.ExportarAsync(formato);

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
                var correoN = new CorreoN {
                    Destinatario = context.Email,
                    Asunto = tipoEnvio.asunto,
                    Cuerpo = tipoEnvio.cuerpo
                };

                correoN.ApplyTags(context.GetTags());

                correoN.FicheroAdjunto = new FicheroAdjunto() {
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