using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Application.Interfaces.Services;
using Application.Services;
using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CorreoController : BaseController<Correo>
    {
        private readonly ICorreoService _correoService;

        public CorreoController(ILogger<CorreoController> logger, 
                                ICorreoService correoService)
        { 
            _logger = logger;
            _correoService = correoService ?? throw new ArgumentNullException(nameof(correoService));
        }

        /// <summary> Envia un correo electronico </summary>
        /// <param name="correo"> Correo con los datos del envio </param> 
        // [Authorize]
        [HttpPost("Enviar")]
        public IActionResult EnviarCorreo([FromBody][Required] Correo correo) {
            try {
                var result = _correoService.EnviarCorreo(correo);
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