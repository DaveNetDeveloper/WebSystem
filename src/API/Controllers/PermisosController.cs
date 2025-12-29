using Application.Interfaces.Services;
using Application.Services;
using Domain.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

using UAParser;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PermisosController : ControllerBase
    {
        protected IConfiguration _config;
        private readonly PermisosService _permisosService;

        public PermisosController(IConfiguration config,
                                  PermisosService permisosService) {
            _config = config;
            _permisosService = permisosService;
        }

        /// <summary> Obtiene las opciones de menú según el rol de acesso </summary>
        //[Authorize]
        //[Authorize(Roles = "Admin,Manager")]
        [HttpGet("ObtenerOpcionesMenu")]
        public async Task<IActionResult> ObtenerOpcionesMenu([FromQuery] string rol) {
            try {
                if (string.IsNullOrEmpty(rol))
                    return BadRequest("No se ha proporcionado el rol del usuario.");

                string baseServerPath = _config["AppConfiguration:apiServer"] + ":" + _config["AppConfiguration:webPort"] + "/";

                var opciones = _permisosService.ObtenerOpcionesMenu(rol, baseServerPath);
                return Ok(opciones);
            }
            catch (Exception ex) {
                return BadRequest($"Error al obtener las opciones de menú para el rol {rol}: {ex.Message}");
            }
        }
    }
}