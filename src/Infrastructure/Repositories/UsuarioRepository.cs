using Domain.Entities; 
using Application.Interfaces.Repositories;
using Application.Interfaces.Common;
using Application.DTOs.Filters;
using Application.Interfaces.DTOs.Filters;
using Infrastructure.Persistence;
using Utilities;

using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
    {
        private readonly ApplicationDbContext _context;

        public UsuarioRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<Usuario>> GetByFiltersAsync(IFilters<Usuario> filters,
                                                                  IQueryOptions<Usuario>? options = null) 
        {
            //var predicate = filters.BuildPredicate(); 
            UsuarioFilters userFilters = ((UsuarioFilters)filters);
            var predicate = PredicateBuilder.New<Usuario>(true);

            if (userFilters.Id.HasValue)
                predicate = predicate.And(u => u.id == userFilters.Id.Value);

            if (!string.IsNullOrEmpty(userFilters.Nombre))
                predicate = predicate.And(u => u.nombre.ToLower() == userFilters.Nombre.ToLower());

            if (!string.IsNullOrEmpty(userFilters.Apellidos))
                predicate = predicate.And(u => u.apellidos.ToLower() == userFilters.Apellidos.ToLower());

            if (!string.IsNullOrEmpty(userFilters.Correo))
                predicate = predicate.And(u => u.correo.ToLower() == userFilters.Correo.ToLower());

            if (userFilters.Activo.HasValue)
                predicate = predicate.And(u => u.activo == userFilters.Activo.Value);

            if (userFilters.Suscrito.HasValue)
                predicate = predicate.And(u => u.suscrito == userFilters.Suscrito.Value);

            if (!string.IsNullOrEmpty(userFilters.Token))
                predicate = predicate.And(u => u.token.ToLower() == userFilters.Token.ToLower());

            if (!string.IsNullOrEmpty(userFilters.Genero))
                predicate = predicate.And(u => u.genero.ToLower() == userFilters.Genero.ToLower());

            var query = _context.Usuarios
                            .AsExpandable()
                            .Where(predicate); 
             
            query = ApplyOrdening(query, options); 
            query = ApplyPagination(query, options);  
            return await query.ToListAsync();  
        }
        public async Task<Usuario?> GetByIdAsync(int id) =>
            await _context.Usuarios.FindAsync(id);

        public async Task<IEnumerable<Usuario>> GetAllAsync() =>
            await _context.Usuarios.ToListAsync();

        public async Task<bool> ActivarSuscripcion(string email)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.correo.ToLower() == email.ToLower());
            if (user == null) return false;
            else {
                user.suscrito = true;  
                await _context.SaveChangesAsync();
                return true;
            }
        }
        public async Task<AuthUser?> Login(string userName, string password)
        {
            var user = _context.Usuarios.SingleOrDefault(x => x.nombre.Trim().ToLower() == userName.Trim().ToLower());

            if (user == null || user.activo == false)
                return null;

            if (PasswordHelper.VerifyPassword(password, user.contrasena)) {
                return new AuthUser {
                    Id = user.id.Value,
                    UserName = user.nombre,
                    Role = string.Empty
                };
            }
            else
                return null;
        } 

        public async Task<bool> ValidarCuenta(string email)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.correo.ToLower() == email.ToLower());
            if (user == null) return false;
            else {
                user.activo = true;
                user.ultimaConexion = DateTime.UtcNow;
                user.token = null;
                user.expiracionToken = null;
                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> CambiarContraseña(string email, string nuevaContraseña)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.correo.ToLower() == email.ToLower());
            if (user == null) return false;
            else {
                user.contrasena = nuevaContraseña; 
                await _context.SaveChangesAsync();
                return true;
            }
        }
       
        public async Task<bool> AddAsync(Usuario usuario) 
        { 
            usuario.id = _context.Usuarios.Count() + 1; 
            await _context.Usuarios.AddAsync(usuario); 
            await _context.SaveChangesAsync();
            return true;
        }
          
        public async Task<bool> UpdateAsync(Usuario usuario)
        {
            var usuarioDB = await _context.Usuarios.FindAsync(usuario.id);
            if (usuarioDB == null)  {
                return false; 
            }
             
            usuarioDB.nombre = usuario.nombre;
            usuarioDB.apellidos = usuario.apellidos;
            usuarioDB.correo = usuario.correo;
            usuarioDB.activo = usuario.activo;
            //usuarioDB.contraseña = usuario.contraseña;
            usuarioDB.fechaNacimiento = usuario.fechaNacimiento;
            usuarioDB.suscrito = usuario.suscrito;
            usuarioDB.ultimaConexion = usuario.ultimaConexion;
            usuarioDB.puntos = usuario.puntos;
            usuarioDB.token = usuario.token;
            usuarioDB.expiracionToken = usuario.expiracionToken;
            usuarioDB.genero = usuario.genero;

            await _context.SaveChangesAsync(); 
            return true;  
        }

        public async Task<bool> ActualizarBalance(int idUsuario, int puntosTransaccion)
        {
            var usuario = await _context.Usuarios.Where(u => u.id == idUsuario).SingleOrDefaultAsync();

            if (usuario != null)
            {
                usuario.puntos += puntosTransaccion;
                await _context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<bool> Remove(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return false;

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return true;
        } 
        
        //
        // Bindings
        //
        public async Task<List<Rol>> GetRolesByUsuarioId(int idUsuario)
        {  
            var usuarioRoles = await _context.Usuarios
                                        .Include(u => u.UsuarioRoles)
                                            .ThenInclude(ur => ur.Rol)
                                        .Include(u => u.UsuarioRoles)
                                            .ThenInclude(ur => ur.Entidad)
                                        .FirstOrDefaultAsync(u => u.id == idUsuario);

            var roles = new List<Rol>();  
            foreach (var usuarioRol in usuarioRoles.UsuarioRoles) { 
                var rol = await _context.Roles.FindAsync(usuarioRol.idrol);
                roles.Add(rol);
            } 
            return roles;
        }

        public async Task<List<Direccion>> GetDireccionesByUsuario(int idUsuario)
        {
            try { 
                var direccionesUsuario = await _context.Usuarios
                         .Where(u => u.id == idUsuario)
                         .SelectMany(u => u.UsuarioDirecciones.Select(ud => ud.Direccion))
                         .ToListAsync();
                 
                if (direccionesUsuario != null && direccionesUsuario.Any()) return direccionesUsuario;
                else return null;
            }
            catch(Exception ex) {
                throw ex; 
            }
        }

        //
        // JOBS
        //

        public async Task<IEnumerable<Usuario>> CheckUnsubscribedUsers()
        {
            try {
                var unsuscribedUsers = await _context.Usuarios
                                       .Where(u => u.suscrito == false)
                                       .OrderBy(u => u.id)
                                       .ToListAsync();

                if (null != unsuscribedUsers && unsuscribedUsers.Any()) return unsuscribedUsers;
                else return null;
            }
            catch (Exception ex) {
                throw ex;
            }
        }       
    }
}