using Application.Common;
using Application.Services;
using Application.Interfaces.Controllers;  
using Application.Interfaces.Services;
using Domain.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{   
    [ApiController]
    [Route("[controller]")]
    public class EmailTokensController : BaseController<EmailToken>, IController<IActionResult, EmailToken, Guid>
    { 
        private readonly IEmailTokenService _emailTokenService;

        public EmailTokensController(ILogger<EmailTokensController> logger, IEmailTokenService emailTokenService)
        {
            _logger = logger;
            _emailTokenService = emailTokenService ?? throw new ArgumentNullException(nameof(emailTokenService));
        }

        [Authorize]
        [HttpGet("ObtenerEmailTokens")]
        public async Task<IActionResult> GetAllAsync()
        {
            var emailTokens = await _emailTokenService.GetAllAsync();
            return (emailTokens != null && emailTokens.Any()) ? Ok(emailTokens) : NoContent();
        }

        [Authorize]
        [HttpGet("ObtenerEmailToken/{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var emailToken = await _emailTokenService.GetByIdAsync(id);
            return emailToken != null ? Ok(emailToken) : NoContent();
        }

        [Authorize]
        [HttpPost("CrearEmailToken")]
        public async Task<IActionResult> AddAsync([FromBody] EmailToken emailToken)
        {
            var nuevaEmailToken = new EmailToken
            {
                userId = emailToken.userId,
                token = emailToken.token,
                emailAction = emailToken.emailAction,
                consumido = emailToken.consumido,
                fechaCreacion = emailToken.fechaCreacion,
                fechaExpiracion = emailToken.fechaExpiracion,
                fechaConsumido = emailToken.fechaConsumido,
                ip = emailToken.ip,
                userAgent = emailToken.userAgent
            }; 

            var result = await _emailTokenService.AddAsync(nuevaEmailToken);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("EmailToken:Crear", "Success"));
                return Ok(result);
            } 
        }

        [Authorize]
        [HttpPut("ActualizarEmailToken")]
        public async Task<IActionResult> UpdateAsync([FromBody] EmailToken emailToken)
        {
            var result = await _emailTokenService.UpdateAsync(emailToken); 
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("EmailToken:Actualizar", "Success"));
                return Ok(result);
            }
        }

        [Authorize]
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(Guid id)
        { 
            try {
                var result = await _emailTokenService.Remove(id);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("EmailToken:Eliminar", "Success"));
                    return Ok(result);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error eliminando el EmailToken, {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("EmailToken:Eliminar", "Error"), id });
            }  
        }

        //[Authorize]
        [HttpGet("CheckEmailToken")]
        public async Task<IActionResult> CheckEmailToken(string emailToken, string email)
        {
            try {
                var result = _emailTokenService.CheckEmailToken(emailToken, email);
                if (result == false) return BadRequest(new { message = "El token o email no son válidos." });
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("EmailToken:CheckEmailToken", "Success"));
                    return Ok(result);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error validando el EmailToken, {id}.", emailToken);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("EmailToken:CheckEmailToken", "Error"), emailToken });
            }
        }

        //[Authorize]
        [HttpPatch("ConsumeEmailToken")]
        public async Task<IActionResult> ConsumeEmailToken(string emailToken, string ip, string userAgent)
        {
            try {
                var consumeResult = _emailTokenService.ConsumeEmailToken(emailToken, ip, userAgent);
                if (consumeResult == false) return NotFound(new { message = "El token no fue encontrado o ya se encuentra consumido." });
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("EmailToken:ConsumeEmailToken", "Success"));
                    return Ok();
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error consumiendo el EmailToken, {id}.", emailToken);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("EmailToken:ConsumeEmailToken", "Error"), emailToken });
            }
        }
    }
}