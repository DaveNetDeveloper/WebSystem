using Application.DTOs.Filters;
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
using System.Threading.Tasks;
using UAParser;

using static Domain.Entities.InAppNotification;
using static Domain.Entities.TipoEnvioCorreo;
using static Domain.Entities.TipoRecompensa;
using static Domain.Entities.TipoTransaccion;

namespace API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("auth")]
    public class AuthController : BaseController<AuthUser>
    {
        private readonly IAuthService _authService;
        private readonly ILoginService _loginService;
        private readonly IEmailTokenService _emailTokenService;
        private readonly ICorreoService _correoService;
        private readonly IInAppNotificationService _inAppNotificationService;
        private readonly IUsuarioService _usuarioService;
        private readonly ITransaccionService _transaccionService;
        private readonly ITipoTransaccionService _tipoTransaccionService;
        private readonly IRecompensaService _recompensaService;

        public AuthController(ILogger<AuthController> logger, 
                              IConfiguration config, 
                              IAuthService authService,
                              ILoginService loginService,
                              IEmailTokenService emailTokenService,
                              ICorreoService correoService,
                              IInAppNotificationService inAppNotificationService,
                              IUsuarioService usuarioService,
                              ITransaccionService transaccionService,
                              ITipoTransaccionService tipoTransaccionService,
                              IRecompensaService recompensaService) {

            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger;
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
            _correoService = correoService ?? throw new ArgumentNullException(nameof(correoService));
            _emailTokenService = emailTokenService ?? throw new ArgumentNullException(nameof(emailTokenService));
            _inAppNotificationService = inAppNotificationService ?? throw new ArgumentNullException(nameof(inAppNotificationService));
            _usuarioService = usuarioService ?? throw new ArgumentNullException(nameof(usuarioService));
            _transaccionService = transaccionService ?? throw new ArgumentNullException(nameof(transaccionService));
            _tipoTransaccionService = tipoTransaccionService ?? throw new ArgumentNullException(nameof(tipoTransaccionService));
            _recompensaService = recompensaService ?? throw new ArgumentNullException(nameof(recompensaService));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns> bool </returns>
        [HttpPost("logout")]
        [AllowAnonymous]
        public IActionResult Logout([FromBody] int idUsuario)
        {
            var result = _authService.DeleteUserToken(idUsuario);
            if (result.Result)
                return Ok(result);
            else
                return NoContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usuarioDTO"></param>
        /// <returns></returns>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] Usuario usuarioDTO)
        {
            var idUsuario = _authService.Register(usuarioDTO)?.Result;
            if (idUsuario is null || !idUsuario.HasValue) return NoContent();
            else {
                // Creamos nuevo EmailToken
                string? emailToken = _emailTokenService.GenerateEmailToken(idUsuario.Value, TipoEnvio.ValidacionCuenta)?.Result;

                // Enviar corrreo para validacion de la nueva cuenta de usuario
                var tipoEnvio = _correoService.ObtenerTiposEnvioCorreo().Result.Where(u => u.nombre == TipoEnvio.ValidacionCuenta).FirstOrDefault();

                var correo = new Correo(tipoEnvio, usuarioDTO.correo, usuarioDTO.nombre, _config["AppConfiguration:LogoURL"]);
                Guid mismoEmailToken = _correoService.EnviarCorreo(correo);

                // Añadimos la notificación InApp de 'Bienvenida' para el nuevo usuario
                var inApp = new InAppNotification { 
                    idUsuario = idUsuario.Value,
                    titulo = "Bienvenidx [_NAME_]",
                    mensaje = "Bienvenidx a tu nueva comunidad!",
                    activo = true,
                    fechaCreacion = DateTime.UtcNow,
                    tipoEnvioInApp = TipoEnvioInApp.Bienvenida
                };
                var result = _inAppNotificationService.AddAsync(inApp);
            }

            bool recomendacionResult = true;
            // Si el usuario viene recomendado por otro usuario
            if (usuarioDTO.codigoRecomendacionRef != null && !string.IsNullOrEmpty(usuarioDTO.codigoRecomendacionRef))
            {
                // Buscamos el usuario de referencia por codigoRecomendacion
                var filter = new UsuarioFilters();
                filter.CodigoRecomendacion = usuarioDTO.codigoRecomendacionRef;
                var usuarioRef = _usuarioService.GetByFiltersAsync(filter)?
                                                .Result
                                                .FirstOrDefault();
                if (usuarioRef != null)
                {
                    var tipoRecompensa = _recompensaService.GetAllTiposRecompensas()
                                                           .Result?
                                                           .Where(tc => tc.nombre == TiposRecompensa.Recomendacion)
                                                           .FirstOrDefault();

                    // Crear recompensa para el usuario recomendador
                    var idRecompensa = _recompensaService.GenerarRecompensa(usuarioRef.id.Value, tipoRecompensa);

                    if (idRecompensa > -1)
                    {
                        // Crear transaccion de puntos por recomendacion 
                        var tipoTransaccion = _tipoTransaccionService.GetAllAsync()
                                                                     .Result
                                                                     .Where(u => u.nombre == TiposTransaccion.PuntosRecomendacion)
                                                                     .SingleOrDefault();
                        var transaccion = new Transaccion {
                            idTipoTransaccion = tipoTransaccion.id,
                            fecha = DateTime.UtcNow,
                            puntos = tipoTransaccion.puntos.Value,
                            idUsuario = usuarioRef.id.Value,
                            nombre = tipoTransaccion.nombre,
                            idProducto = null
                        };
                        await _transaccionService.AddAsync(transaccion);

                        // Crear InAppNotificacion para que el usuario recomendador lo vea en el el proximo login
                        var inAppRef = new InAppNotification {
                            idUsuario = usuarioRef.id.Value,
                            titulo = "Has ganado una recompensa!",
                            mensaje = "Se ha generado una nueva recompensa de tipo [_TIPO_RECOMPENSA_]",
                            activo = true,
                            fechaCreacion = DateTime.UtcNow,
                            tipoEnvioInApp = TipoEnvioInApp.NuevaRecompensa
                        };
                        var result = _inAppNotificationService.AddAsync(inAppRef);

                        // Enviar mail notificando la recompensa al recomendador
                        var tipoEnvioRef = _correoService.ObtenerTiposEnvioCorreo()
                                                         .Result
                                                         .Where(u => u.nombre == TipoEnvio.Recompensa)
                                                         .SingleOrDefault();

                        var correoRef = new Correo(tipoEnvioRef, usuarioRef.correo, usuarioRef.nombre, _config["AppConfiguration:LogoURL"]);
                        Guid emailTokenRef = _correoService.EnviarCorreo(correoRef);
                    }
                }
                else 
                    recomendacionResult = false;
            }
            return Ok(idUsuario != null && recomendacionResult);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost("RequestResetPassword")]
        [AllowAnonymous]
        public IActionResult RequestResetPassword(string email)
        {
            var result = _authService.RequestResetPassword(email);
            if (result is null) return NoContent();

            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
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