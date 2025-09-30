using Application.DTOs.Responses;
using Application.Interfaces.Services;
using Application.Services;
using Domain.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc; 
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using UAParser;

namespace API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : BaseController<AuthUser>
    {
        private readonly IAuthService _authService;
        private readonly ILoginService _loginService;

        public AuthController(ILogger<AuthController> logger, 
                              IConfiguration config, 
                              IAuthService authService,
                              ILoginService loginService) {

            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger;
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            var user = _authService.Login(dto.UserName, dto.Password);
            if (user is null) return Unauthorized();

            var claims = new List<Claim> {
                new(ClaimTypes.NameIdentifier, user.Result.Id.ToString()),
                new(ClaimTypes.Name, user.Result.UserName),
                new(ClaimTypes.Role, user.Result.Role),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:AccessTokenMinutes"]!));

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expires,
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            _authService.RefreshToken(user.Result.Id, jwt, expires); 

            _loginService.AddAsync(new Login { idUsuario = user.Result.Id,
                                               fecha = DateTime.UtcNow, 
                                               ip = ip,
                                               browser = browser.ToString(),
                                               sistemaOperativo = os,
                                               tipoDispositivo = device,
                                               modeloDispositivo = device,
                                               plataforma = device,
                                               idiomaNavegador = primaryLanguage,
                                               pais = null,
                                               region = null });

            return Ok(new { access_token = jwt, token_type = "Bearer", expires_at_utc = expires });
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public IActionResult Register([FromBody] Usuario usuarioDTO)
        {
            // TODO
            // Adaptar el Registro al caso que venga en [UsuarioDTO] el [codigoRecomendacion] informado para
            // activar la recompensa al usuario de referencia

            var result = _authService.Register(usuarioDTO);
            if (result is null) return NoContent();

            return Ok();
        }

        [HttpPost("RequestResetPassword")]
        [AllowAnonymous]
        public IActionResult RequestResetPassword(string email)
        {
            var result = _authService.RequestResetPassword(email);
            if (result is null) return NoContent();

            return Ok();
        }

        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        public IActionResult ResetPassword(string email, string newPassword)
        {
            var result = _authService.ResetPassword(email, newPassword);
            if (result is null) return NoContent();

            return Ok(); 
        } 
    }
}