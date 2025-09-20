using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Domain.Entities; 

namespace Application.Interfaces.Services
{
    public interface IUsuarioService : IService<Usuario, int>
    {
        Task<IEnumerable<Usuario>> GetByFiltersAsync(UsuarioFilters filters,
                                                     IQueryOptions<Usuario>? queryOptions = null);
        Task<bool> ActivarSuscripcion(string email);
        Task<bool> CambiarContraseña(string email, string nuevaContraseña);
        Task<bool> ValidarCuenta(string emai); 
        Task<List<Rol>> GetRolesByUsuarioId(int idUsuario);
        Task<List<Direccion>> GetDireccionesByUsuario(int idUsuario);

        // JOBS
        Task<IEnumerable<Usuario>> CheckUnsubscribedUsers();
    } 
}