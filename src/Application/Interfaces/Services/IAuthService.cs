using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthUser?> Login(string userName, string password); 
        Task<int?> Register(Usuario user);
        Task<bool> RefreshToken(int idUser, string token, DateTime expires);
        Task<Guid> RequestResetPassword(string email);
        Task<bool> ResetPassword(string email, string newPassword);
        Task<bool> DeleteUserToken(int idser);
    }
}