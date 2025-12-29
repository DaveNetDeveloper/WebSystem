using Application.DTOs.Responses;
using Application.Interfaces.Services;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection.Emit;
using System.Text;
using UAParser;
using static Application.Services.PermisosService;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        public DashboardController() {
            
        }

        /// <summary> Obtiene los totales para admin/dashboard según el rol del usuario </summary>
        //[Authorize]
        //[Authorize(Roles = "Admin,Manager")]
        [HttpGet("ObtenerDatosTotales")]
        public async Task<IActionResult> ObtenerDatosTotales([FromServices] IUsuarioService usuarioService,
                                                             [FromServices] ITransaccionService transaccionService,
                                                             [FromServices] IEntidadService entidadService,
                                                             [FromServices] ILoginService loginService) {
            try {
                //if (string.IsNullOrEmpty(rol))
                //    return BadRequest("No se ha proporcionado el rol del usuario.");

                var totales = new DashboardTotalesDTO();
                //switch (rol) {
                //    case string s when s.Equals(Rol.Roles.Manager, StringComparison.OrdinalIgnoreCase):

                //        Aplicar filtros por entidad/es del manager
                //        break;

                //    case string s when s.Equals(Rol.Roles.Admin, StringComparison.OrdinalIgnoreCase):

                var usuarios = await usuarioService.GetAllAsync();
                totales.Usuarios = usuarios.Count();

                var entidades = await entidadService.GetAllAsync();
                totales.Entidades = entidades.Count();

                var transacciones = await transaccionService.GetAllAsync();
                totales.Transacciones = transacciones.Count();

                var visitas = await loginService.GetAllAsync();
                totales.Visitas = visitas.Count();

                //        break;
                //}
                return Ok(totales);
            }
            catch (Exception ex) {
                return BadRequest($"Error al obtener los datos totales para el dashboard: {ex.Message}");
            }
        }
    }
}