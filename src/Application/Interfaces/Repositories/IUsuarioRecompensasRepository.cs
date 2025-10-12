using Domain.Entities;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Common;

namespace Application.Interfaces.Repositories
{
    public interface IUsuarioRecompensasRepository
    {
        Task<IEnumerable<UsuarioRecompensa>> GetUsuariosRecompensasAllAsync();
        Task<UsuarioRecompensa> GetUsuarioRecompensa(int idUsuario, int idRecompensa);
        Task<bool> AddUsuarioRecompensa(UsuarioRecompensa usuarioRecompensa);
        Task<bool> UpdateUsuarioRecompensa(UsuarioRecompensa usuarioRecompensa);
        Task<bool> RemoveUsuarioRecompensa(int idUsuario, int idRecompensa); 
    }
}