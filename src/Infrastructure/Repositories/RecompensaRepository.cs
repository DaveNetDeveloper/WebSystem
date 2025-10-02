using Application.DTOs.Filters;
using Application.DTOs.Responses;
using Application.Interfaces.Common;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class RecompensaRepository : BaseRepository<Recompensa>, IRecompensaRepository
    {
        private readonly ApplicationDbContext _context;

        public RecompensaRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<Recompensa>> GetAllAsync() =>
            await _context.Recompensas.ToListAsync();

        public async Task<IEnumerable<Recompensa>> GetByFiltersAsync(IFilters<Recompensa> filters,
                                                                     IQueryOptions<Recompensa>? options = null)
        {
            RecompensaFilters _filters = ((RecompensaFilters)filters);

            var predicate = PredicateBuilder.New<Recompensa>(true);

            if (_filters.Id.HasValue)
                predicate = predicate.And(u => u.id == _filters.Id.Value);

            if (!string.IsNullOrEmpty(_filters.Nombre))
                predicate = predicate.And(u => u.nombre.ToLower() == _filters.Nombre.ToLower());

            if (_filters.IdEntidad.HasValue)
                predicate = predicate.And(u => u.identidad == _filters.IdEntidad.Value);

            if (_filters.IdTipoRecompensa.HasValue)
                predicate = predicate.And(u => u.idtiporecompensa == _filters.IdTipoRecompensa.Value);

            var query = _context.Recompensas
                            .AsExpandable()
                            .Where(predicate);

            query = ApplyOrdening(query, options);
            query = ApplyPagination(query, options);
            return await query.ToListAsync();
        }

        public async Task<Recompensa?> GetByIdAsync(int id) =>
            await _context.Recompensas.FindAsync(id);

        public async Task<bool> AddAsync(Recompensa recompensa)
        {
            var nuevaRecompensa = new Recompensa {
                id = _context.Recompensas.Count() +1,
                nombre = recompensa.nombre,
                descripcion = recompensa.descripcion,
                identidad = recompensa.identidad,
                idtiporecompensa = recompensa.idtiporecompensa
            }; 
            await _context.Recompensas.AddAsync(nuevaRecompensa);
            await _context.SaveChangesAsync();
            return true;
        } 

        public async Task<bool> UpdateAsync(Recompensa recompensa)
        {
            var updatedRecompensa = await _context.Recompensas.Where(r => r.id == recompensa.id).SingleOrDefaultAsync();
            if (updatedRecompensa == null)
                return false;

            updatedRecompensa.nombre = recompensa.nombre;
            updatedRecompensa.descripcion = recompensa.descripcion;
            updatedRecompensa.identidad = recompensa.identidad;
            updatedRecompensa.idtiporecompensa = recompensa.idtiporecompensa;
            await _context.SaveChangesAsync();
            return true;
        } 

        public async Task<bool> Remove(int id)
        { 
            var recompensaToDelete = await _context.Recompensas.FindAsync(id);

            if (recompensaToDelete == null)
                return false;

            _context.Recompensas.Remove(recompensaToDelete);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Recompensa>> GetRecompensasByUsuario(int idUsuario)
        { 
            var recompensasUsuario = await _context.Recompensas
                    .Include(r => r.UsuarioRecompensas)
                        .ThenInclude(ur => ur.Usuario)
                    .Where(r => r.UsuarioRecompensas.Any(ur => ur.idusuario == idUsuario))
                    .ToListAsync(); 
             
            if (recompensasUsuario != null && recompensasUsuario.Any())
                return recompensasUsuario.OrderBy(ip => ip.id);
            else
                return null;
        }
    }
}