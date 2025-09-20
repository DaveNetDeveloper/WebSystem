using Application.Interfaces;
using Application.DTOs.Filters;
using Domain.Entities;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Utilities;

using System.Collections.Generic;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly ICorreoService _correoService;

        private readonly ILoginService _loginService;

        public AuthService(IUsuarioRepository usuarioRepo, 
                           ICorreoService correoService,
                           ILoginService loginService) {
            _usuarioRepo = usuarioRepo;
            _correoService = correoService;
            _loginService = loginService;
        } 

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

        public async Task<bool> RefreshToken(int idUser, string token, DateTime expires)
        {
            var updatedUSer = await _usuarioRepo.GetByIdAsync(idUser); 
            updatedUSer.token = token;
            updatedUSer.expiracionToken = expires; 
            var result = await _usuarioRepo.UpdateAsync(updatedUSer); 
            return result;
        }

        public async Task<Guid> RequestResetPassword(string email)
        { 
            var correo = new Correo() {
                Destinatario = email,
                Asunto = "Solicitud de recuperación de contraseaña",
                Cuerpo = string.Empty,
                TipoEnvio = TipoEnvioCorreos.ResetContraseña
            }; 
            return _correoService.EnviarCorreo(correo, "", "", "", "", "");  
        }

        public async Task<bool> ResetPassword(string email, string newPassword)
        {
            var filters = new UsuarioFilters();
            filters.Correo = email;

            var usersByEmail = await _usuarioRepo.GetByFiltersAsync(filters);
            if (usersByEmail == null || !usersByEmail.Any()) return false;

            var user = usersByEmail.First();

            user.contraseña = PasswordHelper.HashPassword(newPassword);
            var passwordResult = await _usuarioRepo.UpdateAsync(user);

            var tokenResult = DeleteUserToken(user.id.Value); 

            return true;
        }

        public async Task<bool> DeleteUserToken(int idser)
        {
            var userById = await _usuarioRepo.GetByIdAsync(idser);
            userById.token = null;
            userById.expiracionToken = null;

            var result = await _usuarioRepo.UpdateAsync(userById);
            return result;
        }
    }
}