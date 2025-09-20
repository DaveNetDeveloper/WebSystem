using Domain.Entities;
using Application.DTOs.Filters;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Common; 
using Application.Interfaces.Repositories;
using Infrastructure.Persistence;

using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class SegmentoRepository : BaseRepository<Segmento>, ISegmentoRepository
    {
        private readonly ApplicationDbContext _context;

        public SegmentoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Segmento?> GetByIdAsync(int id) =>
            await _context.Segmentos.FindAsync(id);

        public async Task<IEnumerable<Segmento>> GetAllAsync() =>
           await _context.Segmentos.ToListAsync();

        public async Task<IEnumerable<Segmento>> GetByFiltersAsync(IFilters<Segmento> filters,
                                                                  IQueryOptions<Segmento>? options = null)
        {
            SegmentoFilters _filters = ((SegmentoFilters)filters);

            var predicate = PredicateBuilder.New<Segmento>(true);

            if (_filters.Id.HasValue)
                predicate = predicate.And(u => u.id == _filters.Id.Value);

            if (!string.IsNullOrEmpty(_filters.Nombre))
                predicate = predicate.And(u => u.nombre.ToLower() == _filters.Nombre.ToLower());

            if (_filters.idTipoSegmento != null)
                predicate = predicate.And(u => u.idTipoSegmento == _filters.idTipoSegmento); 

            var query = _context.Segmentos
                            .AsExpandable()
                            .Where(predicate);

            query = ApplyOrdening(query, options);
            query = ApplyPagination(query, options);
            return await query.ToListAsync();
        }

        public async Task<bool> AddAsync(Segmento segmento)
        {
            var toCreate = new Segmento {
                id = (_context.Segmentos.Count() + 1),
                nombre = segmento.nombre,
                descripcion = segmento.descripcion,
                idTipoSegmento = segmento.idTipoSegmento,
                campoRegla = segmento.campoRegla,
                operadorRegla = segmento.operadorRegla,
                valorRegla = segmento.valorRegla
            };

            await _context.Segmentos.AddAsync(toCreate);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(Segmento segmento)
        {
            var dbEntity = await _context.Segmentos.Where(c => c.id == segmento.id).SingleOrDefaultAsync();
            if (dbEntity == null)
                return false;

            dbEntity.nombre = segmento.nombre;
            dbEntity.descripcion = segmento.descripcion;
            dbEntity.idTipoSegmento = segmento.idTipoSegmento;
            dbEntity.campoRegla = segmento.campoRegla;
            dbEntity.operadorRegla = segmento.operadorRegla;
            dbEntity.valorRegla = segmento.valorRegla;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Remove(int id)
        {
            var toDelete = await _context.Segmentos.FindAsync(id);

            if (toDelete == null)
                return false;

            _context.Segmentos.Remove(toDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}