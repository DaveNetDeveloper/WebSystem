using Domain.Entities;
using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Repositories;
using Infrastructure.Persistence;

using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class TipoTransaccionRepository : BaseRepository<TipoTransaccion>, ITipoTransaccionRepository
    {
        private readonly ApplicationDbContext _context;

        public TipoTransaccionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TipoTransaccion?> GetByIdAsync(Guid id) =>
            await _context.TipoTransacciones.FindAsync(id);

        public async Task<IEnumerable<TipoTransaccion>> GetAllAsync() =>
           await _context.TipoTransacciones.ToListAsync();


        public async Task<IEnumerable<TipoTransaccion>> GetByFiltersAsync(IFilters<TipoTransaccion> filters,
                                                                  IQueryOptions<TipoTransaccion>? options = null)
        {
            TipoTransaccionFilters _filters = ((TipoTransaccionFilters)filters);
            _filters = ((TipoTransaccionFilters)filters);

            var predicate = PredicateBuilder.New<TipoTransaccion>(true);

            if (_filters.Id != null)
                predicate = predicate.And(u => u.id == _filters.Id);

            if (!string.IsNullOrEmpty(_filters.Nombre))
                predicate = predicate.And(u => u.nombre.ToLower() == _filters.Nombre.ToLower());

            var query = _context.TipoTransacciones
                            .AsExpandable()
                            .Where(predicate);

            query = ApplyOrdening(query, options);
            query = ApplyPagination(query, options);
            return await query.ToListAsync();
        }

        public async Task<bool> AddAsync(TipoTransaccion tipoTransaccion)
        {
            var toCreate = new TipoTransaccion
            {
                id = new Guid(),
                nombre = tipoTransaccion.nombre,
                descripcion = tipoTransaccion.descripcion
            };

            await _context.TipoTransacciones.AddAsync(toCreate);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(TipoTransaccion tipoTransaccion)
        {
            var dbEntity = await _context.TipoTransacciones.Where(c => c.id == tipoTransaccion.id).SingleOrDefaultAsync();
            if (dbEntity == null)
                return false;

            dbEntity.nombre = tipoTransaccion.nombre;
            dbEntity.descripcion = tipoTransaccion.descripcion;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Remove(Guid id)
        {
            var toDelete = await _context.TipoTransacciones.FindAsync(id);

            if (toDelete == null)
                return false;

            _context.TipoTransacciones.Remove(toDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}