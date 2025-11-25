using Application.DTOs.Filters;
using Application.DTOs.Requests;
using Application.Interfaces;
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Utilities;

namespace Application.Services
{
    public class UsuarioService : IUsuarioService 
    {
        private readonly IUsuarioRepository _repo;

        public UsuarioService(IUsuarioRepository repo) {
            _repo = repo;
        }

        public Task<IEnumerable<Usuario>> GetByFiltersAsync(UsuarioFilters filters,
                                                            IQueryOptions<Usuario>? queryOptions = null)
         => _repo.GetByFiltersAsync(filters, queryOptions); 

        public Task<IEnumerable<Usuario>> GetAllAsync()
            => _repo.GetAllAsync();

        public Task<Usuario?> GetByIdAsync(int id) 
            => _repo.GetByIdAsync(id);

        public Task<bool> ActivarSuscripcion(string email)
           => _repo.ActivarSuscripcion(email);
        
        public async Task<bool> CambiarContrasena(string email, string nuevaContrasena) {
            nuevaContrasena = PasswordHelper.HashPassword(nuevaContrasena);
            return await _repo.CambiarContrasena(email, nuevaContrasena); 
        }   

        public async Task<bool> AddAsync(Usuario usuario) {
            usuario.contrasena = PasswordHelper.HashPassword(usuario.contrasena);
            return await _repo.AddAsync(usuario);
        } 

        public Task<bool> UpdateAsync(Usuario usuario)
            => _repo.UpdateAsync(usuario);

        public Task<bool> Remove(int id)
              => _repo.Remove(id);

        public Task<List<Rol>> GetRolesByUsuarioId(int idUsuario)
             => _repo.GetRolesByUsuarioId(idUsuario);

        public Task<List<Direccion>> GetDireccionesByUsuario(int idUsuario)
            => _repo.GetDireccionesByUsuario(idUsuario);

        public Task<bool> AddRoleAsync(int idUsuario, Guid idRol)
            => _repo.AddRoleAsync(idUsuario, idRol);

        public async Task BajaLogicaAsync(int idUsuario)
            => _repo.BajaLogicaAsync(idUsuario);

        public Task<bool> CompletarPerfil(CompleteProfleRequest completeProfileDTO)
            => _repo.CompletarPerfil(completeProfileDTO);

        //
        // JOBS
        //  

        /// <summary>
        /// 
        /// </summary>
        /// <returns> IEnumerable<Usuario> </returns>
        public Task<IEnumerable<Usuario>> CheckUnsubscribedUsers()
           => _repo.CheckUnsubscribedUsers();
    }
}