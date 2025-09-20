using Application.DTOs.Filters;
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
    public class TipoEntidadRepository : BaseRepository<TipoEntidad>, ITipoEntidadRepository
    {
        private readonly ApplicationDbContext _context;
         
        public TipoEntidadRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<TipoEntidad>> GetByFiltersAsync(IFilters<TipoEntidad> filters,
                                                                  IQueryOptions<TipoEntidad>? options = null)
        {
            TipoEntidadFilters _filters = ((TipoEntidadFilters)filters);

            var predicate = PredicateBuilder.New<TipoEntidad>(true);

            if (_filters.Id.HasValue)
                predicate = predicate.And(u => u.id == _filters.Id.Value);

            if (!string.IsNullOrEmpty(_filters.Nombre))
                predicate = predicate.And(u => u.nombre.ToLower() == _filters.Nombre.ToLower());

            var query = _context.TipoEntidades
                            .AsExpandable()
                            .Where(predicate);

            query = ApplyOrdening(query, options); 
            query = ApplyPagination(query, options); 
            return await query.ToListAsync();
        }

        public async Task<TipoEntidad?> GetByIdAsync(Guid id) =>
            await _context.TipoEntidades.FindAsync(id);

        public async Task<IEnumerable<TipoEntidad>> GetAllAsync() =>
            await _context.TipoEntidades.ToListAsync();


        public async Task<bool> AddAsync(TipoEntidad tipoEntidad)
        {
            var tipoEntidadToCreate = new TipoEntidad {
                id = new Guid(), 
                nombre = tipoEntidad.nombre,
                descripcion = tipoEntidad.descripcion
            };

            await _context.TipoEntidades.AddAsync(tipoEntidadToCreate);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(TipoEntidad tipoEntidad)
        {
            var tipoEntidadDb = await _context.TipoEntidades.Where(c => c.id == tipoEntidad.id).SingleOrDefaultAsync();
            if (tipoEntidadDb == null)
                return false;

            tipoEntidadDb.nombre = tipoEntidad.nombre;
            tipoEntidadDb.descripcion = tipoEntidad.descripcion; 

            await _context.SaveChangesAsync();
            return true;
        } 

        public async Task<bool> Remove(Guid id)
        { 
            var tipoEntidadToDelete = await _context.TipoEntidades.FindAsync(id);

            if (tipoEntidadToDelete == null)
                return false;

            _context.TipoEntidades.Remove(tipoEntidadToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}