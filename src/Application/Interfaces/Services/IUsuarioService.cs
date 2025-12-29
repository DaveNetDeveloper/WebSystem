using Application.DTOs.Filters;
using Application.DTOs.Requests;
using Application.Interfaces.Common;
using Domain.Entities;
using static Utilities.ExporterHelper;

namespace Application.Interfaces.Services
{
    public interface IUsuarioService : IService<Usuario, int>
    {
        Task<IEnumerable<Usuario>> GetByFiltersAsync(UsuarioFilters filters,
                                                     IQueryOptions<Usuario>? queryOptions = null);
        Task<bool> ActivarSuscripcion(string email);
        Task<bool> CambiarContrasena(string email, string nuevaContrasena);
        Task<List<Rol>> GetRolesByUsuarioId(int idUsuario);
        Task<List<Direccion>> GetDireccionesByUsuario(int idUsuario);
        Task<bool> AddRoleAsync(int idUsuario, Guid idRol);
        Task BajaLogicaAsync(int idUsuario);

        Task<bool> CompletarPerfil(CompleteProfleRequest completeProfileDTO);
         

        // JOBS
        Task<IEnumerable<Usuario>> CheckUnsubscribedUsers();
        Task<byte[]> ExportarAsync(ExportFormat formato);
    } 
}