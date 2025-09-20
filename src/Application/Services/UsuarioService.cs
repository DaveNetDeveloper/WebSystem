using Domain.Entities;
using Application.Interfaces.Common;
using Application.DTOs.Filters;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
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
        
        public Task<bool> CambiarContraseña(string email, string nuevaContraseña) {
            nuevaContraseña = PasswordHelper.HashPassword(nuevaContraseña);
            return _repo.CambiarContraseña(email, nuevaContraseña); 
        }   

        public Task<bool> ValidarCuenta(string email)
              => _repo.ValidarCuenta(email);

        public Task<bool> AddAsync(Usuario usuario) {
            usuario.contraseña = PasswordHelper.HashPassword(usuario.contraseña);
            return _repo.AddAsync(usuario);
        } 

        public Task<bool> UpdateAsync(Usuario usuario)
            => _repo.UpdateAsync(usuario);

        public Task<bool> Remove(int id)
              => _repo.Remove(id);

        public Task<List<Rol>> GetRolesByUsuarioId(int idUsuario)
             => _repo.GetRolesByUsuarioId(idUsuario);

        public Task<List<Direccion>> GetDireccionesByUsuario(int idUsuario)
            => _repo.GetDireccionesByUsuario(idUsuario);

        //
        // JOBS
        // 

        public Task<IEnumerable<Usuario>> CheckUnsubscribedUsers()
           => _repo.CheckUnsubscribedUsers();
    }
}