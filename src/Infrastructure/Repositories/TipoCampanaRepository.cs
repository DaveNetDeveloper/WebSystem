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
    public class TipoCampanaRepository : BaseRepository<TipoCampana>, ITipoCampanaRepository
    {
        private readonly ApplicationDbContext _context;

        public TipoCampanaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TipoCampana?> GetByIdAsync(Guid id) =>
            await _context.TipoCampanas.FindAsync(id);

        public async Task<IEnumerable<TipoCampana>> GetAllAsync() =>
           await _context.TipoCampanas.ToListAsync();

        public async Task<IEnumerable<TipoCampana>> GetByFiltersAsync(IFilters<TipoCampana> filters,
                                                                      IQueryOptions<TipoCampana>? options = null)
        {
            TipoCampanaFilters _filters = ((TipoCampanaFilters)filters);

            var predicate = PredicateBuilder.New<TipoCampana>(true);

            if (_filters.Id != null)
                predicate = predicate.And(u => u.id == _filters.Id);

            if (!string.IsNullOrEmpty(_filters.Nombre))
                predicate = predicate.And(u => u.nombre.ToLower() == _filters.Nombre.ToLower());

            if (_filters.Activo.HasValue)
                predicate = predicate.And(u => u.activo == _filters.Activo.Value);

            var query = _context.TipoCampanas
                            .AsExpandable()
                            .Where(predicate);

            query = ApplyOrdening(query, options);
            query = ApplyPagination(query, options);
            return await query.ToListAsync();
        }

        public async Task<bool> AddAsync(TipoCampana tipoCampana)
        {
            var toCreate = new TipoCampana
            {
                id =Guid.NewGuid(),
                nombre = tipoCampana.nombre,
                descripcion = tipoCampana.descripcion,
                activo = tipoCampana.activo,
                objetivo = tipoCampana.objetivo
            };

            await _context.TipoCampanas.AddAsync(toCreate);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(TipoCampana tipoCampana)
        {
            var dbEntity = await _context.TipoCampanas.Where(c => c.id == tipoCampana.id).SingleOrDefaultAsync();
            if (dbEntity == null)
                return false;

            dbEntity.nombre = tipoCampana.nombre;
            dbEntity.descripcion = tipoCampana.descripcion;
            dbEntity.activo = tipoCampana.activo;
            dbEntity.objetivo = tipoCampana.objetivo;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Remove(Guid id)
        {
            var toDelete = await _context.TipoCampanas.FindAsync(id);

            if (toDelete == null)
                return false;

            _context.TipoCampanas.Remove(toDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}