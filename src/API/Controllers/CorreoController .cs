using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Application.Interfaces.Services;
using Application.Services;
using Domain.Entities; 

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CorreoController : BaseController<Correo>
    {
        private readonly ICorreoService _correoService;
        private readonly AppConfiguration _appConfiguration;

        public CorreoController(ILogger<CorreoController> logger, ICorreoService correoService, IOptions<AppConfiguration> options) 
        { 
            _logger = logger;
            _correoService = correoService ?? throw new ArgumentNullException(nameof(correoService));
            _appConfiguration = options.Value ?? throw new ArgumentNullException(nameof(options));
        }
        
        // [Authorize]
        [HttpPost("Enviar")]
        public IActionResult EnviarCorreo([FromBody] Correo correo) {
            try {
                var result = _correoService.EnviarCorreo(correo, "userName", _appConfiguration.ServidorSmtp, _appConfiguration.PuertoSmtp, _appConfiguration.UsuarioSmtp, _appConfiguration.ContraseñaSmtp);
                return Ok("Correo enviado correctamente");
            }
            catch (Exception ex) {
                return BadRequest($"Error al enviar el correo: {ex.Message}");
            }
        }

        // [Authorize]
        [HttpGet("ObtenerTiposEnvioCorreo")]
        public async Task<IActionResult> ObtenerTiposEnvioCorreo()
        {
            var tiposEnvioCorreo = await _correoService.ObtenerTiposEnvioCorreo();
            return tiposEnvioCorreo != null ? Ok(tiposEnvioCorreo) : NoContent();
        } 

    }
}