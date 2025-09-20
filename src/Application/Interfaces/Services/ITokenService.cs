using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface ITokenService
    {
        bool ValidarToken(string token);
        Task<Usuario?> ValidarTokenAsync(string token);
        Task GuardarTokenAsync(int usuarioId, string token);
        Task EliminarTokenAsync(string token);
    }
}