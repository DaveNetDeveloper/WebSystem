using Application.DTOs.Filters;
using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Interfaces.Services;
using Application.Services;
using DocumentFormat.OpenXml.Spreadsheet;
using Domain.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Twilio.TwiML.Voice;
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
                              IRecompensaService recompensaServices
                             ) {

            base._config = config ?? throw new ArgumentNullException(nameof(config));
            base._logger = logger;
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
            _correoService = correoService ?? throw new ArgumentNullException(nameof(correoService));
            _emailTokenService = emailTokenService ?? throw new ArgumentNullException(nameof(emailTokenService));
            _inAppNotificationService = inAppNotificationService ?? throw new ArgumentNullException(nameof(inAppNotificationService));
            _usuarioService = usuarioService ?? throw new ArgumentNullException(nameof(usuarioService));
            _transaccionService = transaccionService ?? throw new ArgumentNullException(nameof(transaccionService));
            _tipoTransaccionService = tipoTransaccionService ?? throw new ArgumentNullException(nameof(tipoTransaccionService));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="refreshTokenDTO"></param>
        /// <returns> IActionResult </returns>
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest refreshTokenDTO)
        {
            if (!string.IsNullOrWhiteSpace(refreshTokenDTO.RefreshToken))
            {
                await _authService.RevokeRefreshTokenAsync(refreshTokenDTO.RefreshToken);
                return Ok();
            }
            else return BadRequest();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="refreshTokenDTO"></param>
        /// <returns></returns>
        [HttpPost("refreshToken")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest refreshTokenDTO)
        {
            RefreshToken refreshToken = await _authService.GetRefreshToken(refreshTokenDTO.RefreshToken);
            if (refreshToken == null || refreshToken.isRevoked == true || refreshToken.expiresAt < DateTime.UtcNow)
                return Unauthorized();

            int idUsuario = refreshToken.idUsuario;

            var user = await _usuarioService.GetByIdAsync(idUsuario);
            if (user == null)
                return Unauthorized();

            string userRole = "";
            var roles = await _usuarioService.GetRolesByUsuarioId(idUsuario);
            if (roles != null && roles.Any())
            {
                Rol role = roles.OrderByDescending(r => r.level).FirstOrDefault();
                userRole = role.nombre;
            }
            else userRole = "";

            //var newAccessToken = await _authService.GenerateAccessToken(stored.idUsuario);

            var claims = new List<Claim> {
                new(ClaimTypes.NameIdentifier, idUsuario.ToString()),
                new(ClaimTypes.Name, user.correo),
                new(ClaimTypes.Role, userRole),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.Now.AddMinutes(int.Parse(_config["Jwt:AccessTokenMinutes"]!));
            //var expires = DateTime.Now.AddMinutes(5);//TODO

            var token = new JwtSecurityToken 
            (
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                notBefore: DateTime.Now,
                expires: expires,
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
              
            return Ok(new
            {
                access_token = jwt,
                token_type = "Bearer",
                expires_at = expires,
                refresh_token = refreshTokenDTO.RefreshToken,
                role = userRole,
                profile = user.idPerfil // TODO Obtener y devolver el "nombre" del perfil en vez del "id"
            }); 

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LoginDto"></param>
        /// <returns> json </returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _authService.Login(dto.Email, dto.Password);
            if (user is null) return NotFound();

            bool isValidRole;
            switch (dto.LoginType) {
                case ILoginService.LoginType.Web:
                    isValidRole = user.Role == Rol.Roles.WebUser;
                    break;

                case ILoginService.LoginType.Admin:
                    
                    isValidRole = user.Role == Rol.Roles.Manager || user.Role == Rol.Roles.Admin;

                    //IEnumerable<Entidad> entidades = await _entidadService.GetAllAsync();
                    //List<int> idsEntidad = new List<int>();

                    //switch (user.Role) {
                    //    case Rol.Roles.Manager:

                    //        var entidad = entidades.Where(e => e.manager.ToLower() == dto.Email.ToLower()).FirstOrDefault();
                    //        if (entidad == null) {
                    //            break; 
                    //        } 
                    //        idsEntidad.Add(entidad.id);
                    //        break;

                    //    case Rol.Roles.Admin:
                    //        idsEntidad.AddRange(entidades.Select(e => e.id));
                    //        break;
                    //}

                    


                    break;
                default:
                    isValidRole = false;
                    break;
            }
            
            if(!isValidRole) return Forbid();

            var claims = new List<Claim> {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Email),
                new(ClaimTypes.Role, user.Role),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            var expires = DateTime.Now.AddMinutes(int.Parse(_config["Jwt:AccessTokenMinutes"]!));
            //var expires = DateTime.Now.AddMinutes(5);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                notBefore: DateTime.Now,
                expires: expires,
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);


            if(dto.LoginType == ILoginService.LoginType.Admin)
            {
                // Crear la cookie segura para guardar la entidad/entidades
                var options = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddHours(2),
                    Path = "/",
                };

                Response.Cookies.Append("Entidades-Cookie", user.Entidades.Count().ToString(), options);
                 
            }

            await _loginService.AddAsync(new Login { idUsuario = user.Id,
                                               fecha = DateTime.UtcNow, 
                                               ip = ip,
                                               browser = browser.ToString(),
                                               sistemaOperativo = os,
                                               tipoDispositivo = device,
                                               modeloDispositivo = device,
                                               plataforma = device,
                                               idiomaNavegador = primaryLanguage,
                                               pais = null,
                                               region = null,
                                               loginType = dto.LoginType
            });

            var refreshToken = await _authService.GenerateRefreshToken(user.Id);

            return Ok(new {
                access_token = jwt,
                refresh_token = refreshToken,
                token_type = "Bearer",
                expires_at = expires,
                role = user.Role,
                profile = user.Profile,
                entidades = user.Entidades
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param> 
        /// <returns> json </returns>
        [AllowAnonymous]
        [HttpPatch("ValidarCuenta")]
        public async Task<IActionResult> ValidarCuenta([FromQuery] string email, string emailToken)
        {
            try  
            {
                var emailTokenResut = await _emailTokenService.CheckEmailToken(emailToken, email);
                if (!emailTokenResut) return NotFound();

                var validateResult = await _authService.ValidarCuenta(email);
                if (!validateResult) return NotFound(); 

                var authUser = await _authService.Login(email, string.Empty, true);
                if (authUser is null) {
                    return NotFound();
                }

                var claims = new List<Claim> {
                    new(ClaimTypes.NameIdentifier, authUser.Id.ToString()),
                    new(ClaimTypes.Name, authUser.Email),
                    new(ClaimTypes.Role, authUser.Role),
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expires = DateTime.Now.AddMinutes(int.Parse(_config["Jwt:AccessTokenMinutes"]!));
                //var expires = DateTime.Now.AddMinutes(5);

                var token = new JwtSecurityToken (
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims: claims,
                    notBefore: DateTime.Now,
                    expires: expires,
                    signingCredentials: creds
                );
                var jwt = new JwtSecurityTokenHandler().WriteToken(token);

                await _loginService.AddAsync(new Login
                {
                    idUsuario = authUser.Id,
                    fecha = DateTime.UtcNow,
                    ip = ip,
                    browser = browser.ToString(),
                    sistemaOperativo = os,
                    tipoDispositivo = device,
                    modeloDispositivo = device,
                    plataforma = device,
                    idiomaNavegador = primaryLanguage,
                    pais = null,
                    region = null,
                    loginType = ILoginService.LoginType.Web.ToString()
                });

                var refreshToken = await _authService.GenerateRefreshToken(authUser.Id);

                // Enviar corrreo: 'Cuenta validada correctamente'
                var tiposEnvioCorreo = await _correoService.ObtenerTiposEnvioCorreo();
                var tipoCorreo = tiposEnvioCorreo.Where(u => u.nombre.Trim() == TipoEnvioCorreo.TipoEnvio.Bienvenida.Trim())
                                                 .Single();

                var usuarios = await _usuarioService.GetAllAsync();
                var usuario = usuarios.Where(u => u.correo.ToLower() == email.ToLower())
                                      .SingleOrDefault();

                var correo = new Correo(tipoCorreo, 
                                        email, 
                                        usuario.nombre, 
                                        _config["AppConfiguration:LogoURL"]);
                
                var res = _correoService.EnviarCorreo(correo);

                 return Ok(new  { 
                    access_token = jwt,
                    refresh_token = refreshToken,
                    token_type = "Bearer", 
                    expires_at = expires, 
                    role = authUser.Role,
                    profile = authUser.Profile,
                    entidades = string.Empty
                });
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validando la cuenta del usuario, {email}.", email);
                
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error validando la cuenta del usuario, {email}", email });
                //return StatusCode(StatusCodes.Status500InternalServerError,
                    // new { message = MessageProvider.GetMessage("Auth:ValidarCuenta", "Error"), email });
            }
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
            var idUsuario = await  _authService.Register(usuarioDTO);
            if (idUsuario is null || !idUsuario.HasValue) return NoContent();
            else {

                // Obtener Rol por?

                // Asociamos el rol por defeto al usuario (WeUser)
                var rolAdded = await _usuarioService.AddRoleAsync(idUsuario.Value, Rol.RolesIDs.WebUser);
                
                // Creamos nuevo EmailToken
                string? emailToken = await _emailTokenService.GenerateEmailToken(idUsuario.Value, TipoEnvio.ValidacionCuenta);

                // Enviar corrreo para validacion de la nueva cuenta de usuario
                var tiposEnvio = await _correoService.ObtenerTiposEnvioCorreo();
                var tipoEnvio = tiposEnvio.Where(u => u.nombre == TipoEnvio.ValidacionCuenta)
                                          .FirstOrDefault();

                var correo = new Correo(tipoEnvio, 
                                        usuarioDTO.correo, 
                                        usuarioDTO.nombre, 
                                        _config["AppConfiguration:LogoURL"], 
                                        Guid.Parse(emailToken));

                Guid? resultEmailToken = _correoService.EnviarCorreo(correo);

                // Añadimos la notificación InApp de 'Bienvenida' para el nuevo usuario
                var inApp = new InAppNotification { 
                    idUsuario = idUsuario.Value,
                    titulo = "Bienvenidx [_NAME_]",
                    mensaje = "Bienvenidx a tu nueva comunidad!",
                    activo = true,
                    fechaCreacion = DateTime.UtcNow,
                    tipoEnvioInApp = TipoEnvioInApp.Bienvenida
                };
                var result = await _inAppNotificationService.AddAsync(inApp);
            }

            bool recomendacionResult = true;
            // Si el usuario viene recomendado por otro usuario
            if (usuarioDTO.codigoRecomendacionRef != null && !string.IsNullOrEmpty(usuarioDTO.codigoRecomendacionRef))
            {
                // Buscamos el usuario de referencia por codigoRecomendacion
                var filter = new UsuarioFilters();
                filter.CodigoRecomendacion = usuarioDTO.codigoRecomendacionRef;
                var usuarios = await _usuarioService.GetByFiltersAsync(filter);
                var usuarioRef = usuarios.FirstOrDefault();

                if (usuarioRef != null)
                {
                    var tiposRecompensa = await _recompensaService.GetAllTiposRecompensas();
                    var tipoRecompensa = tiposRecompensa.Where(tc => tc.nombre == TiposRecompensa.Recomendacion)
                                                        .FirstOrDefault();

                    // Crear recompensa para el usuario recomendador
                    var idRecompensa = await _recompensaService.GenerarRecompensa(usuarioRef.id.Value, tipoRecompensa);

                    if (idRecompensa > -1)
                    {
                        var tiposTransaccion = await _tipoTransaccionService.GetAllAsync();
                        var tipoTransaccion = tiposTransaccion
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
                        var result = await _inAppNotificationService.AddAsync(inAppRef);

                        // Enviar mail notificando la recompensa al recomendador
                        var tiposEnvioRef = await _correoService.ObtenerTiposEnvioCorreo();
                        var tipoEnvioRef = tiposEnvioRef.Where(u => u.nombre == TipoEnvio.Recompensa)
                                                        .SingleOrDefault();

                        var correoRef = new Correo(tipoEnvioRef, usuarioRef.correo, usuarioRef.nombre, _config["AppConfiguration:LogoURL"]);
                        Guid? emailTokenRef = _correoService.EnviarCorreo(correoRef);
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
        /// <returns>Guid?</returns>
        [HttpPost("RequestResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> RequestResetPassword([FromQuery] string email)
        {
            var result = await _authService.RequestResetPassword(email);
            if (result is null) return NoContent();

            return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="newPassword"></param>
        /// <returns>bool</returns>
        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromQuery] string email, string newPassword)
        {
            var result = await _authService.ResetPassword(email, newPassword);
            if (!result) return NoContent();


            // Enviar corrreo: 'Contraseña cambiada correctamente'
            var tiposEnvioCorreo = await _correoService.ObtenerTiposEnvioCorreo();
            var tipoCorreo = tiposEnvioCorreo.Where(u => u.nombre.Trim() == TipoEnvioCorreo.TipoEnvio.ContrasenaCambiada.Trim())
                                             .Single();

            var usuarios = await _usuarioService.GetAllAsync();
            var usuario = usuarios.Where(u => u.correo.ToLower() == email.ToLower())
                                  .SingleOrDefault();

            var correo = new Correo(tipoCorreo,
                                    email,
                                    usuario.nombre,
                                    _config["AppConfiguration:LogoURL"]);

            var res = _correoService.EnviarCorreo(correo);

            return Ok(result); 
        } 
    }
}