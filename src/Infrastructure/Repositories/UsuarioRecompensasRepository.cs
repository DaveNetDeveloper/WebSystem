using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;

using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public class UsuarioRecompensasRepository : BaseRepository<UsuarioRecompensasRepository>,
                                                IUsuarioRecompensasRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public UsuarioRecompensasRepository(ApplicationDbContext context) {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns> IEnumerable<UsuarioRecompensa> </returns>
        public async Task<IEnumerable<UsuarioRecompensa>> GetUsuariosRecompensasAllAsync() =>
            await _context.UsuarioRecompensas.ToListAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="idRecompensa"></param>
        /// <returns> UsuarioRecompensa </returns>
        public async Task<UsuarioRecompensa> GetUsuarioRecompensa(int idUsuario, int idRecompensa) 
            => await _context.UsuarioRecompensas.SingleOrDefaultAsync(u => u.idusuario == idUsuario && u.idrecompensa == idRecompensa);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recompensa"></param>
        /// <returns> bool </returns>
        public async Task<bool> AddUsuarioRecompensa(UsuarioRecompensa usuarioRecompensa) {
             
            var nuevaRecompensa = new UsuarioRecompensa
            {
                idusuario = usuarioRecompensa.idusuario,
                idrecompensa = usuarioRecompensa.idrecompensa,
                fecha = DateTime.UtcNow
            };

            _context.UsuarioRecompensas.Add(nuevaRecompensa);
            _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usuarioRecompensa"></param>
        /// <returns> bool </returns>
        public async Task<bool> UpdateUsuarioRecompensa(UsuarioRecompensa usuarioRecompensa)
        {
            var updated = _context.UsuarioRecompensas.Where(r => r.idusuario == usuarioRecompensa.idusuario && 
                                                                 r.idrecompensa == usuarioRecompensa.idrecompensa).SingleOrDefault();
            if (updated == null)
                return false;

            updated.idusuario = usuarioRecompensa.idusuario;
            updated.idrecompensa = usuarioRecompensa.idrecompensa;
            updated.fecha = DateTime.UtcNow; 

            _context.SaveChanges(); 
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="idRecompensa"></param>
        /// <returns> bool </returns>
        public async Task<bool> RemoveUsuarioRecompensa(int idUsuario, int idRecompensa)
        { 
            var toDelete = _context.UsuarioRecompensas.SingleOrDefault(u => u.idusuario == idUsuario && u.idrecompensa == idRecompensa);

            if (toDelete == null)
                return false;

            _context.UsuarioRecompensas.Remove(toDelete);
            _context.SaveChanges();

            return true;
        } 
    }
}