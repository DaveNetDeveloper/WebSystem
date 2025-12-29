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
    public class TipoActividadRepository : BaseRepository<TipoActividad>, ITipoActividadRepository
    {
        private readonly ApplicationDbContext _context;
         
        public TipoActividadRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<TipoActividad>> GetByFiltersAsync(IFilters<TipoActividad> filters,
                                                                        IQueryOptions<TipoActividad>? options = null)
        {
            TipoActividadFilters _filters = ((TipoActividadFilters)filters);

            var predicate = PredicateBuilder.New<TipoActividad>(true);

            if (_filters.Id.HasValue)
                predicate = predicate.And(u => u.id == _filters.Id.Value);

            if (!string.IsNullOrEmpty(_filters.Nombre))
                predicate = predicate.And(u => u.nombre.ToLower() == _filters.Nombre.ToLower());

            var query = _context.TipoActividades
                            .AsExpandable()
                            .Where(predicate);

            query = ApplyOrdening(query, options); 
            query = ApplyPagination(query, options); 
            return await query.ToListAsync();
        }

        public async Task<TipoActividad?> GetByIdAsync(Guid id) =>
            await _context.TipoActividades.FindAsync(id);

        public async Task<IEnumerable<TipoActividad>> GetAllAsync() =>
            await _context.TipoActividades.OrderBy(t => t.nombre).ToListAsync();

        public async Task<bool> AddAsync(TipoActividad tipoEntidad)
        {
            var tipoEntidadToCreate = new TipoActividad
            {
                id = Guid.NewGuid(), 
                nombre = tipoEntidad.nombre,
                descripcion = tipoEntidad.descripcion
            };

            await _context.TipoActividades.AddAsync(tipoEntidadToCreate);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(TipoActividad tipoEntidad)
        {
            var tipoEntidadDb = await _context.TipoActividades.Where(c => c.id == tipoEntidad.id).SingleOrDefaultAsync();
            if (tipoEntidadDb == null)
                return false;

            tipoEntidadDb.nombre = tipoEntidad.nombre;
            tipoEntidadDb.descripcion = tipoEntidad.descripcion; 

            await _context.SaveChangesAsync();
            return true;
        } 

        public async Task<bool> Remove(Guid id)
        { 
            var tipoEntidadToDelete = await _context.TipoActividades.FindAsync(id);

            if (tipoEntidadToDelete == null)
                return false;

            _context.TipoActividades.Remove(tipoEntidadToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}