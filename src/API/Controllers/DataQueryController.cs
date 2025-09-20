using Application.DTOs.DataQuery;
using Application.Interfaces.Services; 

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;  

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataQueryController : ControllerBase
    { 
        private readonly IDataQueryService _dataQueryService; 

        public DataQueryController(IDataQueryService dataQueryService) {
             
            _dataQueryService = dataQueryService ?? throw new ArgumentNullException(nameof(dataQueryService));  
        }

        //[Authorize(Policy = "RequireAdmin")]
        [HttpGet("ObtenerUsuariosInactivos")]
        public async Task<IActionResult> ObtenerUsuariosInactivos()
        {
            IReadOnlyList<vUsuariosInactivosDTO> queryResult = await _dataQueryService.ObtenerUsuariosInactivos();
            return (queryResult != null && queryResult.Any()) ? Ok(queryResult) : NoContent();
        }

        //[Authorize(Policy = "RequireAdmin")]
        [HttpGet("ObtenerActividadUsuarios")]
        public async Task<IActionResult> ObtenerActividadUsuarios()
        {
            IReadOnlyList<vActividadUsuariosDTO> queryResult = await _dataQueryService.ObtenerActividadUsuarios();
            return (queryResult != null && queryResult.Any()) ? Ok(queryResult) : NoContent();
        }

        //[Authorize(Policy = "RequireAdmin")]
        [HttpGet("ObtenerUsuariosDispositivos")]
        public async Task<IActionResult> ObtenerUsuariosDispositivos()
        {
            IReadOnlyList<vUsuariosDispositivosDTO> queryResult = await _dataQueryService.ObtenerUsuariosDispositivos();
            return (queryResult != null && queryResult.Any()) ? Ok(queryResult) : NoContent();
        }

        //[Authorize(Policy = "RequireAdmin")]
        [HttpGet("ObtenerUsuariosIdiomas")]
        public async Task<IActionResult> ObtenerUsuariosIdiomas()
        {
            IReadOnlyList<vUsuariosIdiomasDTO> queryResult = await _dataQueryService.ObtenerUsuariosIdiomas();
            return (queryResult != null && queryResult.Any()) ? Ok(queryResult) : NoContent();
        }

        //[Authorize(Policy = "RequireAdmin")]
        [HttpGet("ObtenerRolesUsuarios")]
        public async Task<IActionResult> ObtenerRolesUsuarios()
        {
            IReadOnlyList<vRolesUsuariosDTO> queryResult = await _dataQueryService.ObtenerRolesUsuarios();
            return (queryResult != null && queryResult.Any()) ? Ok(queryResult) : NoContent();
        }

        //[Authorize(Policy = "RequireAdmin")]
        [HttpGet("ObtenerVisitasTipoDispositivo")]
        public async Task<IActionResult> ObtenerVisitasTipoDispositivo()
        {
            IReadOnlyList<vVisitasTipoDispositivoDTO> queryResult = await _dataQueryService.ObtenerVisitasTipoDispositivo();
            return (queryResult != null && queryResult.Any()) ? Ok(queryResult) : NoContent();
        }

        //[Authorize(Policy = "RequireAdmin")]
        [HttpGet("ObtenerTotalUltimasTransacciones")]
        public async Task<IActionResult> ObtenerTotalUltimasTransacciones()
        {
            IReadOnlyList<vTotalUltimasTransaccionesDTO> queryResult = await _dataQueryService.ObtenerTotalUltimasTransacciones();
            return (queryResult != null && queryResult.Any()) ? Ok(queryResult) : NoContent();
        }

    }
}