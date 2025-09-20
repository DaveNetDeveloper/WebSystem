using Domain.Entities;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Common;

namespace Application.Interfaces.Repositories
{
    public interface ITokenRepository
    {
        /// <summary>
        /// Devuelve si el token existe y es valido
        /// </summary>
        bool ValidarToken(string token); 
        Task<Usuario?> GetUsuarioByTokenAsync(string token); 
        Task SaveTokenAsync(int usuarioId, string token); 
        Task RemoveTokenAsync(string token);
    }
}