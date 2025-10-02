using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Repositories;
using Domain.Entities; 
using Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using LinqKit;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class TipoRecompensasRepository : BaseRepository<TipoRecompensa>, 
                                             ITipoRecompensasRepository
    {
        private readonly ApplicationDbContext _context;

        public TipoRecompensasRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<TipoRecompensa>> GetByFiltersAsync(IFilters<TipoRecompensa> filters,
                                                                         IQueryOptions<TipoRecompensa>? options = null)
        {
            TipoRecompensaFilters _filters = ((TipoRecompensaFilters)filters); 

            var predicate = PredicateBuilder.New<TipoRecompensa>(true);

            if (_filters.Id.HasValue)
                predicate = predicate.And(u => u.id == _filters.Id.Value);

            if (!string.IsNullOrEmpty(_filters.Nombre))
                predicate = predicate.And(u => u.nombre.ToLower() == _filters.Nombre.ToLower()); 

            if (_filters.Activo.HasValue)
                predicate = predicate.And(u => u.activo == _filters.Activo.Value);

            var query = _context.TipoRecompensas
                            .AsExpandable()
                            .Where(predicate);

            query = ApplyOrdening(query, options); 
            query = ApplyPagination(query, options); 
            return await query.ToListAsync();
        }

        public async Task<TipoRecompensa?> GetByIdAsync(Guid id) =>
            await _context.TipoRecompensas.FindAsync(id);

        public async Task<IEnumerable<TipoRecompensa>> GetAllAsync() =>
           await _context.TipoRecompensas.OrderBy(t => t.nombre).ToListAsync();

        public async Task<bool> AddAsync(TipoRecompensa tipoRecompensa)
        {
            var toCreate = new TipoRecompensa
            {
                id = Guid.NewGuid(),
                nombre = tipoRecompensa.nombre,
                descripcion = tipoRecompensa.descripcion,
                activo = tipoRecompensa.activo
            };

            await _context.TipoRecompensas.AddAsync(toCreate);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(TipoRecompensa tipoRecompensa)
        {
            var tipoRecompensaDB = await _context.TipoRecompensas
                                                   .Where(c => c.id == tipoRecompensa.id)
                                                   .SingleOrDefaultAsync();
            if (tipoRecompensaDB == null)
                return false;

            tipoRecompensaDB.nombre = tipoRecompensa.nombre;
            tipoRecompensaDB.descripcion = tipoRecompensa.descripcion;
            tipoRecompensaDB.activo = tipoRecompensa.activo;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Remove(Guid id)
        {
            var toDelete = await _context.TipoRecompensas.FindAsync(id);

            if (toDelete == null)
                return false;

            _context.TipoRecompensas.Remove(toDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
