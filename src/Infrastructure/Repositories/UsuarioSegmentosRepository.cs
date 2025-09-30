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
    public class UsuarioSegmentosRepository : BaseRepository<UsuarioSegmentosRepository>, 
                                              IUsuarioSegmentosRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public UsuarioSegmentosRepository(ApplicationDbContext context) {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns> IEnumerable<UsuarioSegmentos </returns>
        public async Task<IEnumerable<UsuarioSegmentos>> GetUsuariosSegmentosAllAsync() =>
            await _context.UsuarioSegmentos.ToListAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="idSegmento"></param>
        /// <returns> UsuarioSegmentos </returns>
        public UsuarioSegmentos GetSegmentoByIdUsuario(int idUsuario, int idSegmento) 
            =>  _context.UsuarioSegmentos.SingleOrDefault(u => u.idUsuario == idUsuario && u.idSegmento == idSegmento);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usuarioSegmentos"></param>
        /// <returns> bool </returns>
        public bool AddUsuarioSegmento(UsuarioSegmentos usuarioSegmentos) {
             
            var nuevo = new UsuarioSegmentos {
                idUsuario = usuarioSegmentos.idUsuario,
                idSegmento = usuarioSegmentos.idSegmento,
                fecha = DateTime.UtcNow
            };

            _context.UsuarioSegmentos.Add(nuevo);
            _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toUpdate"></param>
        /// <returns> bool </returns>
        public bool UpdateUsuarioSegmento(UsuarioSegmentos toUpdate)
        {
            var updated = _context.UsuarioSegmentos.Where(r => r.idUsuario == toUpdate.idUsuario && 
                                                               r.idSegmento == toUpdate.idSegmento).SingleOrDefault();
            if (updated == null)
                return false;

            updated.idUsuario = toUpdate.idUsuario;
            updated.idSegmento = toUpdate.idSegmento;
            updated.fecha = DateTime.UtcNow; 

            _context.SaveChanges(); 
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="idSegmento"></param>
        /// <returns> bool </returns>
        public bool RemoveUsuarioSegmento(int idUsuario, int idSegmento)
        { 
            var toDelete = _context.UsuarioSegmentos.SingleOrDefault(u => u.idUsuario == idUsuario && u.idSegmento == idSegmento);

            if (toDelete == null)
                return false;

            _context.UsuarioSegmentos.Remove(toDelete);
            _context.SaveChanges();

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idSegmento"></param>
        /// <returns> IEnumerable<Usuario> </returns>
        public async Task<IEnumerable<Usuario>> GetUsuariosBySegmento(int idSegmento)
        {
            var usuarioSegmentos = _context.UsuarioSegmentos.Include(u => u.Usuario)
                                                            .Include(s => s.Segmento)
                                                            .Where(us => us.idSegmento == idSegmento);
            
            if (usuarioSegmentos != null && usuarioSegmentos.Any()) {
                var usuariosResult = new List<Usuario>();
                foreach (var usuarioSegmento in usuarioSegmentos) {
                    usuariosResult.Add(usuarioSegmento.Usuario);
                }
                return usuariosResult.OrderBy(ip => ip.id);
            }
            return null;
        }
    }
}