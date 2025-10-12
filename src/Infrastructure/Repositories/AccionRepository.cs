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
    public class AccionRepository : BaseRepository<Accion>, IAccionRepository
    {
        private readonly ApplicationDbContext _context;
         
        public AccionRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<Accion>> GetByFiltersAsync(IFilters<Accion> filters,
                                                                 IQueryOptions<Accion>? options = null)
        {
            AccionFilters _filters = ((AccionFilters)filters);

            var predicate = PredicateBuilder.New<Accion>(true);

            if (_filters.Id.HasValue)
                predicate = predicate.And(u => u.id == _filters.Id.Value);

            if (_filters.Activo.HasValue)
                predicate = predicate.And(u => u.activo == _filters.Activo.Value);

            if (!string.IsNullOrEmpty(_filters.Nombre))
                predicate = predicate.And(u => u.nombre.ToLower() == _filters.Nombre.ToLower());

            if (!string.IsNullOrEmpty(_filters.TipoAccion))
                predicate = predicate.And(u => u.tipoAccion.ToLower() == _filters.TipoAccion.ToLower());

            var query = _context.Acciones
                            .AsExpandable()
                            .Where(predicate);

            query = ApplyOrdening(query, options);
            query = ApplyPagination(query, options);
            return await query.ToListAsync();
        }

        public async Task<Accion?> GetByIdAsync(Guid id) =>
            await _context.Acciones.FindAsync(id);
 
        public async Task<IEnumerable<Accion>> GetAllAsync() =>
            await _context.Acciones.ToListAsync();
        
        public async Task<bool> AddAsync(Accion accion)
        {
            var nuevaAccion = new Accion
            {
                id = Guid.NewGuid(),
                nombre = accion.nombre,
                descripcion = accion.descripcion, 
                tipoAccion = accion.tipoAccion,
                activo = accion.activo
            };
             
            await _context.Acciones.AddAsync(nuevaAccion);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Accion accion)
        {
            var toUpdate = await _context.Acciones.Where(c => c.id == accion.id).SingleOrDefaultAsync();
            if (toUpdate == null)
                return false;

            toUpdate.nombre = accion.nombre;
            toUpdate.descripcion = accion.descripcion;
            toUpdate.tipoAccion = accion.tipoAccion;
            toUpdate.activo = accion.activo;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Remove(Guid id)
        { 
            var toDelete = await _context.Acciones.FindAsync(id);

            if (toDelete == null)
                return false;

            _context.Acciones.Remove(toDelete);
            await _context.SaveChangesAsync();
            return true;
        } 
    }
}