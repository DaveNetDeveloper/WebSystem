using Application.Common;
using Application.DTOs.Filters;
using Application.Interfaces.Controllers;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Services;
using Application.Services;
using Domain.Entities;
using Utilities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting; 
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : BaseController<Usuario>, IController<IActionResult, Usuario, int>
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IEmailTokenService _emailTokenService;
        private readonly ICorreoService _correoService;
        private readonly AppConfiguration _appConfiguration;
        public UsuariosController(ILogger<UsuariosController> logger, 
                                  IUsuarioService usuarioService,
                                  IEmailTokenService emailTokenService,
                                  ITokenService tokenService,
                                  ICorreoService correoService,
                                  IOptions<AppConfiguration> options) 
        {
            base._logger = logger;  
            base._tokenService = tokenService;
            _appConfiguration = options.Value ?? throw new ArgumentNullException(nameof(options));
            _usuarioService = usuarioService ?? throw new ArgumentNullException(nameof(usuarioService));
            _correoService = correoService ?? throw new ArgumentNullException(nameof(correoService));
            _emailTokenService = emailTokenService ?? throw new ArgumentNullException(nameof(emailTokenService));
        }

        [Authorize(Policy = "RequireAdmin")] 
        [EnableRateLimiting("UsuariosLimiter")]
        [HttpGet("ObtenerUsuarios")]
        public async Task<IActionResult> GetAllAsync()
        { 
            try { 
                //var headerToken = Request.Headers["Authorization"].FirstOrDefault();  
                //var isUserAuthenticated = User.Identity.IsAuthenticated;  
                //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                //var role = User.FindFirstValue(ClaimTypes.Role);
              
                var usuarios = await _usuarioService.GetAllAsync();
                return (usuarios != null && usuarios.Any()) ? Ok(usuarios) : NoContent(); 
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error obteniendo todos los usuarios.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("Usuario:GetAll", "Error") });
            }
        }

        //[AllowAnonymous]
        [Authorize(Policy = "RequireAdmin")]
        [HttpGet("FiltrarUsuarios")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] UsuarioFilters filters, // TODO: IFilters<Usuario>
                                                           [FromQuery] int? page,
                                                           [FromQuery] int? pageSize,
                                                           [FromQuery] string? orderBy,
                                                           [FromQuery] bool descending = false) {
            try  {
                var _filters = filters as UsuarioFilters;
                if (_filters is null) 
                    throw new InvalidOperationException("El filtro recibido no es de tipo 'UsuarioFilters'.");

                var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);
                var filteredUsers = await _usuarioService.GetByFiltersAsync(_filters, queryOptions);
                return Ok(filteredUsers);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error filtrando usuario.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("Usuario:GetByFilters", "Error")});
            }
        }

        //// TODO To Delete
        //[Authorize(Policy = "RequireAdmin")]
        //[HttpGet("ObtenerUsuario/{id}")]
        //public async Task<IActionResult> GetByIdAsync(int id) 
        //{
        //    var resulToken = IsValidToken();
        //    try {  
        //        var usuario = await _usuarioService.GetByIdAsync(id);
        //        if (usuario == null) return NoContent();

        //        _logger.LogInformation(MessageProvider.GetMessage("Usuario:ObtenerPorId", "Success")); 
        //        return Ok(usuario);
        //    }
        //    catch (Exception ex) {
        //        _logger.LogError(ex, "Error obteniendo un usuario por Id, {id}.", id);
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //                         new { message = MessageProvider.GetMessage("Usuario:ObtenerPorId", "Error"), id });
        //    } 
        //}

        [Authorize(Policy = "RequireAdmin")]
        [HttpPost("CrearUsuario")]
        public async Task<IActionResult> AddAsync([FromBody] Usuario usuario)
        {
            try {
                var nuevoUsuario = new Usuario {
                    id = 10,
                    nombre = usuario.nombre,
                    correo = usuario.correo,
                    apellidos = usuario.apellidos,
                    activo = false,
                    contrasena = usuario.contrasena,
                    fechaNacimiento = usuario.fechaNacimiento.ToUniversalTime(),
                    suscrito = usuario.suscrito,
                    fechaCreacion = DateTime.UtcNow,
                    ultimaConexion = null,
                    puntos = 0,//defaultPuntos,
                    token= null,
                    expiracionToken = null,
                    genero = usuario.genero
                };

                var result = await _usuarioService.AddAsync(nuevoUsuario); 
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("Usuario:Crear", "Success"));
                    return Ok(result);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error creando el usuario, {id}.", usuario.nombre);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("Usuario:Crear", "Error"), usuario.nombre });
            }
        }

        [Authorize(Policy = "RequireAdmin")]
        [HttpPut("ActualizarUsuario")]
        public async Task<IActionResult> UpdateAsync([FromBody] Usuario usuario) 
        {
            try {
                var result = await _usuarioService.UpdateAsync(usuario);

                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("Usuario:Actualizar", "Success"));
                    return Ok(result);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error actualizandousuario, {id}.", usuario.id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("Usuario:Actualizar", "Error"), usuario.id });
            }
        }

        [Authorize]
        [HttpPatch("CambiarContraseña")]
        public async Task<IActionResult> CambiarContraseña([FromQuery] string email, string nuevaContraseña) 
        {
            try {
                var result = await _usuarioService.CambiarContraseña(email, nuevaContraseña);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("Usuario:CambiarContraseña", "Success"));
                    return Ok(result);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error cambiando la contraseña del usuario, {email}.", email);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("Usuario:CambiarContraseña", "Error"), email });
            }
        }

        [AllowAnonymous]
        [HttpPatch("ValidarCuenta")]
        public async Task<IActionResult> ValidarCuenta([FromQuery] string email) 
        {
            try { 
                var result = await _usuarioService.ValidarCuenta(email);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("Usuario:ValidarCuenta", "Success"));
                    return Ok(result);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error validando la cuenta del usuario, {email}.", email);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("Usuario:ValidarCuenta", "Error"), email });
            }
        }

        [AllowAnonymous]
        [HttpPatch("ActivacionSuscripcion")]
        public async Task<IActionResult> ActivacionSuscripcion([FromQuery] string token, string email) 
        {
            try {
                var validToken = FormatValidationHelper.GetValidGuidFronString(token);
                var validEmail = FormatValidationHelper.IsValidEmail(email);
                 
                bool isValidToken = false;
                if (validToken.HasValue && validEmail) {
                    isValidToken =  _emailTokenService.CheckEmailToken(validToken.ToString(), email);
                }

                if (!isValidToken)
                    return BadRequest(new { message = "El token o email no son válidos." });
                else {
                    var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
                    var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

                    var activationResult = await _usuarioService.ActivarSuscripcion(email);
                    if (activationResult == false) return NotFound(new { message = "El usuario no ha sido encontrado." });
                    else {
                       var consumeResult = _emailTokenService.ConsumeEmailToken(validToken.ToString(), ip, userAgent);
                        if(consumeResult) {

                            //
                            // TODO enviar corrreo: Bienvenido a nuestra newsletter
                            //

                            var tiposEnvioCorreo = _correoService.ObtenerTiposEnvioCorreo();

                            //var usuario = _usuarioService.GetAllAsync().Result.Where(u => u.correo == email).SingleOrDefault();

                            var correo = new Correo { Destinatario = email, 
                                                      Asunto = "Bienvenido a nuestra Newsletter", 
                                                      TipoEnvio = TipoEnvioCorreos.SuscripciónActivada, 
                                                      Cuerpo = "Gracias por unirte a la mejor comunidad cerca de ti." };

                            _correoService.EnviarCorreo(correo,
                                                        "userName", //usuario?.nombre ?? email,
                                                        EncodeDecodeHelper.GetDecodeValue(_appConfiguration.ServidorSmtp),
                                                        EncodeDecodeHelper.GetDecodeValue(_appConfiguration.PuertoSmtp),
                                                        EncodeDecodeHelper.GetDecodeValue(_appConfiguration.UsuarioSmtp),
                                                        EncodeDecodeHelper.GetDecodeValue(_appConfiguration.ContraseñaSmtp));
                             
                            _logger.LogInformation(MessageProvider.GetMessage("Usuario:ActivacionSuscripcion", "Success"));
                            return Ok(consumeResult);
                        }
                        else
                            return NotFound(new { message = "El token no fue encontrado o ya ha sido consumido previamente." });
                    }
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error activando la cuenta del usuario, {email}.", email);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("Usuario:ActivacionSuscripcion", "Error"), email });
            }
        }

        [Authorize(Policy = "RequireAdmin")]
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            try {
                var result = await _usuarioService.Remove(id); 
                if (result == false) return NotFound();
                else { 
                    _logger.LogInformation(MessageProvider.GetMessage("Usuario:Eliminar", "Success"));
                    return Ok(result);
                } 
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error eliminando el usuario, {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("Usuario:Eliminar", "Error"), id });
            }  
        }

        [Authorize]
        [HttpGet("GetDireccionesByUsuario/{idUsuario}")]
        public async Task<IActionResult> GetDireccionesByUsuario(int idUsuario)
        {
            try {
                var dreccionesByUser = await _usuarioService.GetDireccionesByUsuario(idUsuario);

                if (dreccionesByUser == null || !dreccionesByUser.Any()) return NotFound();
                else { 
                    return Ok(dreccionesByUser);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error obteniendo las direcciones del usuario {id}.", idUsuario);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("Usuario:GetDirecciones", "Error"), idUsuario });
            }
        }

        [Authorize]
        [HttpGet("GetRolesByUsuario/{idUsuario}")]
        public async Task<IActionResult> GetRolesByUsuarioId(int idUsuario)
        {
            try {
                var rolesByUser = await _usuarioService.GetRolesByUsuarioId(idUsuario); 
                if (rolesByUser == null || !rolesByUser.Any()) return NotFound();
                else {
                    return Ok(rolesByUser);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error obteniendo los roles del usuario {id}.", idUsuario);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("Usuario:GetRoles", "Error"), idUsuario });
            }
        }  
    }
}