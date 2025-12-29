using Domain.Entities;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Common;

namespace Application.Interfaces.Repositories
{
    public interface IUsuarioSegmentosRepository
    {
        Task<IEnumerable<UsuarioSegmentos>> GetUsuariosSegmentosAllAsync();
        UsuarioSegmentos GetSegmentoByIdUsuario(int idUsuario, int idSegmento);
        bool AddUsuarioSegmento(UsuarioSegmentos usuarioSegmentos);
        bool UpdateUsuarioSegmento(UsuarioSegmentos toUpdate);
        bool RemoveUsuarioSegmento(int idUsuario, int idSegmento);

        Task<IEnumerable<Usuario>> GetUsuariosBySegmento(int idUsuario);
    }
}