using Application.DTOs.Filters;
using Application.DTOs.Requests;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using DocumentFormat.OpenXml.Spreadsheet;
using Domain.Entities;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Utilities;

namespace Application.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly IRefreshTokenRepository _refreshTokenRepo;
        private readonly IEntidadService _entidadService;

        private readonly ICorreoService _correoService;
        private readonly ILoginService _loginService;
        private readonly MailConfiguration _appConfig;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="usuarioRepo"></param>
        /// <param name="correoService"></param>
        /// <param name="loginService"></param>
        public AuthService(IUsuarioRepository usuarioRepo,
                           ICorreoService correoService,
                           ILoginService loginService,
                           IOptions<MailConfiguration> configOptions,
                           IRefreshTokenRepository refreshTokenRepo,
                           IEntidadService entidadService) {
            _usuarioRepo = usuarioRepo;
            _correoService = correoService;
            _loginService = loginService;
            _appConfig = configOptions.Value;
            _refreshTokenRepo = refreshTokenRepo;
            _entidadService = entidadService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns> AuthUser </returns>
        public async Task<AuthUser?> Login(string email, 
                                           string password, 
                                           bool force = false) {

            //var hashed = PasswordService.HashPassword(password); 
            var authUser = await _usuarioRepo.Login(email, password, force);

            if (authUser != null) {
                var usuarioRoles = await _usuarioRepo.GetRolesByUsuarioId(authUser.Id);
                if (usuarioRoles != null && usuarioRoles.Any()) {

                    var maxRole = usuarioRoles.OrderByDescending(r => r.level).FirstOrDefault();
                    authUser.Role = maxRole.nombre;

                    switch (authUser.Role) { 

                        case Rol.Roles.Admin:

                            var entitiesByAdmin = await _entidadService.GetAllAsync();
                            authUser.Entidades = entitiesByAdmin.Select(e => e.id.ToString()).ToArray();
                            break;
                        case Rol.Roles.Manager:
                            
                            var entitiesByManager = await _entidadService.GetByFiltersAsync(new EntidadFilters { Manager = email });
                            authUser.Entidades = entitiesByManager.Select(e => e.id.ToString()).ToArray();
                            break;
                        default:
                            authUser.Entidades = null;
                            break;
                    }  
                }
                else authUser.Role = null;
            }
            return authUser;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns> int? </returns>
        public async Task<int?> Register(Usuario usuario)
        {
            // comprobamos si ya existe un usuario con el mismo correo electrónico
            var filter = new UsuarioFilters { Correo = usuario.correo };
            var existingUser = await _usuarioRepo.GetByFiltersAsync(filter); 

            if (existingUser != null && existingUser.Any()) return null;

            // sobrescribimos con valores por defecto para nuevos usuarios
            usuario.SetInitValuesForNewUser();

            // encriptamos la contraseña introducida por el usuario
            usuario.contrasena = PasswordHelper.HashPassword(usuario.contrasena);

            // Creamos el usuario
            var result = await _usuarioRepo.AddAsync(usuario);
            if (result) {
                var nuevoUsuario = _usuarioRepo.GetByFiltersAsync(filter)
                                               .Result
                                               .SingleOrDefault();
                if(nuevoUsuario != null) 
                    return nuevoUsuario.id;
            }
            return null;
        }
         
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns> Guid del Token asocuado al correo enviado</returns>
        public async Task<Guid?> RequestResetPassword(string email)
        {
            var tipoEnvioCorreo = _correoService.ObtenerTiposEnvioCorreo()
                                                .Result.Where(u => u.nombre.Trim() == TipoEnvioCorreo.TipoEnvio.CambiarContrasena.Trim())
                                                .SingleOrDefault();
            var filters = new UsuarioFilters() { 
                Correo = email 
            };
            var usersByEmail = await _usuarioRepo.GetByFiltersAsync(filters);
            var nombre = usersByEmail.SingleOrDefault().nombre;

            var correo = new Correo(tipoEnvioCorreo, email, nombre, _appConfig.LogoURL);
            return _correoService.EnviarCorreo(correo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="newPassword"></param>
        /// <returns> bool </returns>
        public async Task<bool> ResetPassword(string email, string newPassword)
        {
            var filters = new UsuarioFilters();
            filters.Correo = email.Trim().ToLower();

            var usersByEmail = await _usuarioRepo.GetByFiltersAsync(filters);
            if (usersByEmail == null || !usersByEmail.Any()) return false;

            var user = usersByEmail.FirstOrDefault();

            user.contrasena = PasswordHelper.HashPassword(newPassword);
            var passwordResult = await _usuarioRepo.UpdateAsync(user);

            //TODO: Delete User Token?

            return true;
        }

        public async Task<string> GenerateRefreshToken(int usuarioId)
        {
            var random = RandomNumberGenerator.GetBytes(64);// Generar raw token seguro (base64)
            var refreshTokenRaw = Convert.ToBase64String(random);

            var refreshHash = EncryptHelper.ComputeSha256Base64(refreshTokenRaw);// Hashear (SHA256 -> base64)

            var entity = new RefreshToken
            {
                idUsuario = usuarioId,
                refreshToken = refreshHash,
                //expiresAt = DateTime.Now.AddDays(7),
                expiresAt  = DateTime.UtcNow.AddDays(7),
                isRevoked = false
            };

            var resutl = await _refreshTokenRepo.AddAsync(entity);

            // Devolver el valor RAW al cliente (NO el hash)
            return refreshTokenRaw;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param> 
        /// <returns> bool </returns>
        public async Task<bool> ValidarCuenta(string email) 
            => await _usuarioRepo.ValidarCuenta(email);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idUser"></param>
        /// <param name="token"></param>
        /// <param name="expires"></param>
        /// <returns> bool </returns>
        public async Task<RefreshToken> GetRefreshToken(string refreshToken) 
            => await _refreshTokenRepo.GetByTokenHashAsync(EncryptHelper.ComputeSha256Base64(refreshToken)); 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idser"></param>
        /// <returns> bool </returns>
        public async Task<bool> RevokeRefreshTokenAsync(string refreshToken)
            => await _refreshTokenRepo.RevokeRefreshTokenAsync(EncryptHelper.ComputeSha256Base64(refreshToken)); 

        /// <summary>
        /// 
        /// </summary>
        public static class AuthenticationSchemes
        {
            public const string Admin = "Admin";
            public const string Test = "Test";
            public const string Default = "Bearer";
        }
    }
}