using Application.DTOs.Requests;
using Application.Interfaces.Common;
using Application.Interfaces.DTOs.Filters;
using Domain.Entities; 
using static System.Net.Mime.MediaTypeNames;

namespace Application.Interfaces.Repositories
{
    public interface IUsuarioRepository : IRepository<Usuario, int>
    {
        Task<IEnumerable<Usuario>> GetByFiltersAsync(IFilters<Usuario> filters, IQueryOptions<Usuario>? options = null);
        Task<AuthUser?> Login(string email, string contrasena, bool force = false);
        Task<int?> Register(Usuario user);
        Task<bool> CambiarContrasena(string email, string nuevaContrasena); 
        Task<bool> ActivarSuscripcion(string email);
        Task<bool> ValidarCuenta(string emai);
        Task<bool> ActualizarBalance(int idUsuario, int? puntosTransaccion);
        Task<List<Rol>> GetRolesByUsuarioId(int id);
        Task<List<Direccion>> GetDireccionesByUsuario(int idUsuario);

        Task<bool> CompletarPerfil(CompleteProfleRequest completeProfileDTO);

        Task<bool> AddRoleAsync(int idUsuario, Guid idRol);

        Task BajaLogicaAsync(int idUsuario);

        // JOBS
        Task<IEnumerable<Usuario>> CheckUnsubscribedUsers();
    }
}