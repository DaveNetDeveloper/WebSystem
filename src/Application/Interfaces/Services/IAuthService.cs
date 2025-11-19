using Application.DTOs.Requests;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthUser?> Login(string email, string password, bool force = false);
        Task<bool> ValidarCuenta(string emai);
        Task<int?> Register(Usuario user);
        Task<Guid> RequestResetPassword(string email);
        Task<bool> ResetPassword(string email, string newPassword);


        Task<RefreshToken> GetRefreshToken(string refreshToken);
        Task<string> GenerateRefreshToken(int idUsuario); 
        Task<bool> RevokeRefreshTokenAsync(string refreshToken);
    }
}