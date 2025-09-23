using Domain.Entities; 
using static System.Net.Mime.MediaTypeNames;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Common;

namespace Application.Interfaces.Repositories
{
    public interface IUsuarioRepository : IRepository<Usuario, int>
    {
        Task<IEnumerable<Usuario>> GetByFiltersAsync(IFilters<Usuario> filters, IQueryOptions<Usuario>? options = null);
        Task<AuthUser?> Login(string email, string contrasena);
        Task<bool> CambiarContrasena(string email, string nuevaContrasena); 
        Task<bool> ActivarSuscripcion(string email);
        Task<bool> ValidarCuenta(string emai);
        Task<bool> ActualizarBalance(int idUsuario, int puntosTransaccion);
        Task<List<Rol>> GetRolesByUsuarioId(int id);
        Task<List<Direccion>> GetDireccionesByUsuario(int idUsuario);

        // JOBS
        Task<IEnumerable<Usuario>> CheckUnsubscribedUsers();
    }
}