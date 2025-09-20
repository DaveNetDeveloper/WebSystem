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
    public class TipoSegmentoRepository : BaseRepository<TipoSegmento>, ITipoSegmentoRepository
    {
        private readonly ApplicationDbContext _context;

        public TipoSegmentoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TipoSegmento?> GetByIdAsync(Guid id) =>
            await _context.TipoSegmentos.FindAsync(id);

        public async Task<IEnumerable<TipoSegmento>> GetAllAsync() =>
           await _context.TipoSegmentos.ToListAsync();

        public async Task<IEnumerable<TipoSegmento>> GetByFiltersAsync(IFilters<TipoSegmento> filters,
                                                                  IQueryOptions<TipoSegmento>? options = null)
        {
            TipoSegmentoFilters _filters = ((TipoSegmentoFilters)filters);

            var predicate = PredicateBuilder.New<TipoSegmento>(true);

            if (_filters.Id != null)
                predicate = predicate.And(u => u.id == _filters.Id);

            if (!string.IsNullOrEmpty(_filters.Nombre))
                predicate = predicate.And(u => u.nombre.ToLower() == _filters.Nombre.ToLower());

            var query = _context.TipoSegmentos
                            .AsExpandable()
                            .Where(predicate);

            query = ApplyOrdening(query, options);
            query = ApplyPagination(query, options);
            return await query.ToListAsync();
        }

        public async Task<bool> AddAsync(TipoSegmento tipoSegmento)
        {
            var toCreate = new TipoSegmento {
                id = new Guid(),
                nombre = tipoSegmento.nombre,
                descripcion = tipoSegmento.descripcion
            };

            await _context.TipoSegmentos.AddAsync(toCreate);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(TipoSegmento tipoSegmento)
        {
            var dbEntity = await _context.TipoSegmentos.Where(c => c.id == tipoSegmento.id).SingleOrDefaultAsync();
            if (dbEntity == null)
                return false;

            dbEntity.nombre = tipoSegmento.nombre;
            dbEntity.descripcion = tipoSegmento.descripcion;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Remove(Guid id)
        {
            var toDelete = await _context.TipoSegmentos.FindAsync(id);

            if (toDelete == null)
                return false;

            _context.TipoSegmentos.Remove(toDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}