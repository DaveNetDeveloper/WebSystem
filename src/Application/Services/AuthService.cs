using Application.Interfaces;
using Application.DTOs.Filters;
using Domain.Entities;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Utilities;

using System.Collections.Generic;

namespace Application.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly ICorreoService _correoService; 
        private readonly ILoginService _loginService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="usuarioRepo"></param>
        /// <param name="correoService"></param>
        /// <param name="loginService"></param>
        public AuthService(IUsuarioRepository usuarioRepo, 
                           ICorreoService correoService,
                           ILoginService loginService) {
            _usuarioRepo = usuarioRepo;
            _correoService = correoService;
            _loginService = loginService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns> AuthUser </returns>
        public async Task<AuthUser?> Login(string userName, string password)
        {
            //var hashed = PasswordService.HashPassword(password); 
            var authUser = _usuarioRepo.Login(userName, password); 
            
            if(authUser != null && authUser.Result != null) { 
                var usuarioRoles = await _usuarioRepo.GetRolesByUsuarioId(authUser.Result.Id);
                if (usuarioRoles != null && usuarioRoles.Any()) { 
                
                    var maxRole = usuarioRoles.OrderByDescending(r => r.level).FirstOrDefault();
                    authUser.Result.Role = maxRole.nombre;
                }
                else authUser.Result.Role = null;
            }
            return authUser.Result;
        } 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idUser"></param>
        /// <param name="token"></param>
        /// <param name="expires"></param>
        /// <returns> bool </returns>
        public async Task<bool> RefreshToken(int idUser, string token, DateTime expires)
        {
            var updatedUSer = await _usuarioRepo.GetByIdAsync(idUser); 
            updatedUSer.token = token;
            updatedUSer.expiracionToken = expires; 
            var result = await _usuarioRepo.UpdateAsync(updatedUSer); 
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns> Guid del Token asocuado al correo enviado</returns>
        public async Task<Guid> RequestResetPassword(string email)
        {
            var tipoEnvioCorreo = _correoService.ObtenerTiposEnvioCorreo().Result.Where(u => u.nombre == "ResetContrasena").SingleOrDefault();

            // TODO: Recibir config en constructor por DI y obtener logoUrl de ahí
            var logoUrl = "https://www.getautismactive.com/wp-content/uploads/2021/01/Test-Logo-Circle-black-transparent.png";

            var correo = new Correo(tipoEnvioCorreo, email, "", logoUrl);
            // TODO por probar sin parametros 
            return _correoService.EnviarCorreo(correo);  // TODO
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
            filters.Correo = email;

            var usersByEmail = await _usuarioRepo.GetByFiltersAsync(filters);
            if (usersByEmail == null || !usersByEmail.Any()) return false;

            var user = usersByEmail.First();

            user.contrasena = PasswordHelper.HashPassword(newPassword);
            var passwordResult = await _usuarioRepo.UpdateAsync(user);

            var tokenResult = DeleteUserToken(user.id.Value); 

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idser"></param>
        /// <returns> bool </returns>
        public async Task<bool> DeleteUserToken(int idser)
        {
            var userById = await _usuarioRepo.GetByIdAsync(idser);
            userById.token = null;
            userById.expiracionToken = null;

            var result = await _usuarioRepo.UpdateAsync(userById);
            return result;
        }

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