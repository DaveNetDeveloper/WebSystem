using Application.Interfaces.Services;
using Application.Services;
using Domain.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class AppConfigurationController : ControllerBase
    {
        protected IConfiguration _config;

        public AppConfigurationController(IConfiguration config)
        {
            _config = config;
        }

        /// <summary> Obtiene las variables de configuracion </summary>
        //[Authorize]
        [HttpGet("GetConfiguration")]
        public IActionResult GetConfiguration() {
            try {
                var globalConfig = new GlobalConfiguration () {
                    BaseUrl = _config["GlobalConfiguration:BaseUrl"],
                    Port = _config["GlobalConfiguration:Port"],
                    
                    AccessTokenCookieName = _config["GlobalConfiguration:AccessTokenCookieName"],
                    RefreshTokenCookieName = _config["GlobalConfiguration:RefreshTokenCookieName"],
                    ProfileCookieName = _config["GlobalConfiguration:RoleCookieName"],
                    RoleCookieName = _config["GlobalConfiguration:ProfileCookieName"],
                    
                    ControllerName = new GlobalConfiguration.ControllerNames() { 
                        AuthController = _config["GlobalConfiguration:ControllerNames:AuthController"],
                        UsuariosController = _config["GlobalConfiguration:ControllerNames:UsuariosController"],
                        PerfilesController = _config["GlobalConfiguration:ControllerNames:PerfilesController"],
                        RolesController = _config["GlobalConfiguration:ControllerNames:RolesController"],
                        NotificationsController = _config["GlobalConfiguration:ControllerNames:NotificationsController"],
                        EmailTokensController = _config["GlobalConfiguration:ControllerNames:EmailTokensController"],
                        InAppNotificationController = _config["GlobalConfiguration:ControllerNames:InAppNotificationController"],
                        LoginsController = _config["GlobalConfiguration:ControllerNames:LoginsController"],
                        ProductosController = _config["GlobalConfiguration:ControllerNames:ProductosController"]
                    }
                };
                return Ok(globalConfig);
            }
            catch (Exception ex) {
                return BadRequest($"Error al obtener la configuración global: {ex.Message}");
            }
        }
    }
}