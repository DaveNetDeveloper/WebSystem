using Application.DTOs.DataQuery;
using Application.Interfaces.Services;
using Domain.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using static Application.Services.DataQueryService;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataQueryController : ControllerBase
    { 
        private readonly IDataQueryService _dataQueryService;
        private readonly IExportService _exportService;
        private readonly ExportConfiguration _exportConfig;

        public DataQueryController(IDataQueryService dataQueryService,
                                   IExportService exportService,
                                   IOptions<ExportConfiguration> options) {
           
            _dataQueryService = dataQueryService ?? throw new ArgumentNullException(nameof(dataQueryService));
            _exportService = exportService ?? throw new ArgumentNullException(nameof(exportService));
            _exportConfig = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        /// Exportar vista a Excel
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns> File to download </returns>
        [HttpGet("Exportar")]
        //[Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> Exportar([FromQuery] DataQueryType dataQueryType)
        {
            var file = await _exportService.ExportarAsync(dataQueryType);
            var fileName = $"DataQuery_{dataQueryType.ToString()}_{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx"; 
            return File(file, _exportConfig.ExcelContentType, fileName);
        }

        /// <summary>
        /// Vista que obtiene ...
        /// </summary>
        /// <returns> vUsuariosInactivosDTO view entity </returns>
        //[Authorize(Policy = "RequireAdmin")]
        [HttpGet("ObtenerUsuariosInactivos")]
        public async Task<IActionResult> ObtenerUsuariosInactivos()
        {
            IReadOnlyList<vUsuariosInactivosDTO> queryResult = await _dataQueryService.ObtenerUsuariosInactivos();
            return (queryResult != null && queryResult.Any()) ? Ok(queryResult) : NoContent();
        }

        /// <summary>
        /// Vista que obtiene ...
        /// </summary>
        /// <returns> vActividadUsuariosDTO view entity </returns>
        //[Authorize(Policy = "RequireAdmin")]
        [HttpGet("ObtenerActividadUsuarios")]
        public async Task<IActionResult> ObtenerActividadUsuarios()
        {
            IReadOnlyList<vActividadUsuariosDTO> queryResult = await _dataQueryService.ObtenerActividadUsuarios();
            return (queryResult != null && queryResult.Any()) ? Ok(queryResult) : NoContent();
        }

        /// <summary>
        /// Vista que obtiene ...
        /// </summary>
        /// <returns> vUsuariosDispositivosDTO view entity </returns>
        //[Authorize(Policy = "RequireAdmin")]
        [HttpGet("ObtenerUsuariosDispositivos")]
        public async Task<IActionResult> ObtenerUsuariosDispositivos()
        {
            IReadOnlyList<vUsuariosDispositivosDTO> queryResult = await _dataQueryService.ObtenerUsuariosDispositivos();
            return (queryResult != null && queryResult.Any()) ? Ok(queryResult) : NoContent();
        }

        /// <summary>
        /// Vista que obtiene ...
        /// </summary>
        /// <returns> vUsuariosIdiomasDTO view entity </returns>
        //[Authorize(Policy = "RequireAdmin")]
        [HttpGet("ObtenerUsuariosIdiomas")]
        public async Task<IActionResult> ObtenerUsuariosIdiomas()
        {
            IReadOnlyList<vUsuariosIdiomasDTO> queryResult = await _dataQueryService.ObtenerUsuariosIdiomas();
            return (queryResult != null && queryResult.Any()) ? Ok(queryResult) : NoContent();
        }

        /// <summary>
        /// Vista que obtiene ...
        /// </summary>
        /// <returns> vRolesUsuariosDTO view entity </returns>
        //[Authorize(Policy = "RequireAdmin")]
        [HttpGet("ObtenerRolesUsuarios")]
        public async Task<IActionResult> ObtenerRolesUsuarios()
        {
            IReadOnlyList<vRolesUsuariosDTO> queryResult = await _dataQueryService.ObtenerRolesUsuarios();
            return (queryResult != null && queryResult.Any()) ? Ok(queryResult) : NoContent();
        }

        /// <summary>
        /// Vista que obtiene ...
        /// </summary>
        /// <returns> vVisitasTipoDispositivoDTO view entity </returns>
        //[Authorize(Policy = "RequireAdmin")]
        [HttpGet("ObtenerVisitasTipoDispositivo")]
        public async Task<IActionResult> ObtenerVisitasTipoDispositivo()
        {
            IReadOnlyList<vVisitasTipoDispositivoDTO> queryResult = await _dataQueryService.ObtenerVisitasTipoDispositivo();
            return (queryResult != null && queryResult.Any()) ? Ok(queryResult) : NoContent();
        }

        /// <summary>
        /// Vista que obtiene ...
        /// </summary>
        /// <returns> vTotalUltimasTransaccionesDTO view entity </returns>
        //[Authorize(Policy = "RequireAdmin")]
        [HttpGet("ObtenerTotalUltimasTransacciones")]
        public async Task<IActionResult> ObtenerTotalUltimasTransacciones()
        {
            IReadOnlyList<vTotalUltimasTransaccionesDTO> queryResult = await _dataQueryService.ObtenerTotalUltimasTransacciones();
            return (queryResult != null && queryResult.Any()) ? Ok(queryResult) : NoContent();
        }

        /// <summary>
        /// Vista que obtiene ...
        /// </summary>
        /// <returns> vCampanasUsuariosDTO view entity </returns>
        //[Authorize(Policy = "RequireAdmin")]
        [HttpGet("ObtenerObtenerCampanasUsuarios")]
        public async Task<IActionResult> ObtenerCampanasUsuarios()
        {
            IReadOnlyList<vCampanasUsuariosDTO> queryResult = await _dataQueryService.ObtenerCampanasUsuarios();
            return (queryResult != null && queryResult.Any()) ? Ok(queryResult) : NoContent();
        }

        /// <summary>
        /// Vista que obtiene toda la informacion relacionada con los usuarios
        /// </summary>
        /// <returns> vAllUserDataDTO view entity </returns>
        //[Authorize(Policy = "RequireAdmin")]
        [HttpGet("ObtenerAllUserData")]
        public async Task<IActionResult> ObtenerAllUserData()
        {
            IReadOnlyList<vAllUserDataDTO> queryResult = await _dataQueryService.ObtenerAllUserData();
            return (queryResult != null && queryResult.Any()) ? Ok(queryResult) : NoContent();
        }

        /// <summary>
        /// Vista que obtiene toda la informacion relacionada con los detalles de las campañas
        /// </summary>
        /// <returns> vAllUserDataDTO view entity </returns>
        //[Authorize(Policy = "RequireAdmin")]
        [HttpGet("ObtenerAllCampanasData")]
        public async Task<IActionResult> ObtenerAllCampanasData()
        {
            IReadOnlyList<vAllCampanasDataDTO> queryResult = await _dataQueryService.ObtenerAllCampanasData();
            return (queryResult != null && queryResult.Any()) ? Ok(queryResult) : NoContent();
        }

        /// <summary>
        /// Vista que obtiene toda la informacion relacionada con las asistencias a las actividades
        /// </summary>
        /// <returns> vAsistenciaActividadesDTO view entity </returns>
        //[Authorize(Policy = "RequireAdmin")]
        [HttpGet("ObtenerAsistenciaActividades")]
        public async Task<IActionResult> ObtenerAsistenciaActividades()
        {
            IReadOnlyList<vAsistenciaActividadesDTO> queryResult = await _dataQueryService.ObtenerAsistenciaActividades();
            return (queryResult != null && queryResult.Any()) ? Ok(queryResult) : NoContent();
        }
    }
}