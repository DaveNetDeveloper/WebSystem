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
    public class CampanaRepository : BaseRepository<Campana>, ICampanaRepository
    {
        private readonly ApplicationDbContext _context;
         
        public CampanaRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<Campana>> GetByFiltersAsync(IFilters<Campana> filters,
                                                                  IQueryOptions<Campana>? options = null)
        { 
            CampanaFilters _filters = ((CampanaFilters)filters);

            var predicate = PredicateBuilder.New<Campana>(true);

            if (_filters.Id.HasValue)
                predicate = predicate.And(u => u.id == _filters.Id.Value);

            if (_filters.IdTipoCampana.HasValue)
                predicate = predicate.And(u => u.idTipoCampana == _filters.IdTipoCampana.Value);

            if (_filters.Activo.HasValue)
                predicate = predicate.And(u => u.activo == _filters.Activo.Value);

            if (!string.IsNullOrEmpty(_filters.Nombre))
                predicate = predicate.And(u => u.nombre.ToLower() == _filters.Nombre.ToLower());

            if (!string.IsNullOrEmpty(_filters.Frecuencia))
                predicate = predicate.And(u => u.frecuencia.ToLower() == _filters.Frecuencia.ToLower());

            var query = _context.Campanas
                            .AsExpandable()
                            .Where(predicate);

            query = ApplyOrdening(query, options);
            query = ApplyPagination(query, options);
            return await query.ToListAsync();
        }

        public async Task<Campana?> GetByIdAsync(int id) =>
            await _context.Campanas.FindAsync(id);
 
        public async Task<IEnumerable<Campana>> GetAllAsync() =>
            await _context.Campanas.ToListAsync();
        
        public async Task<bool> AddAsync(Campana campana)
        {
            var nuevaCampana = new Campana
            {
                id = (_context.Campanas.Count() + 1),
                nombre = campana.nombre,
                descripcion = campana.descripcion, 
                idTipoCampana = campana.idTipoCampana,
                activo = campana.activo, 
                frecuencia = campana.frecuencia,
                fechaInicio = campana.fechaInicio,
                fechaFin = campana.fechaFin
            };

            await _context.Campanas.AddAsync(nuevaCampana);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Campana campana)
        {
            var toUpdate = await _context.Campanas.Where(c => c.id == campana.id).SingleOrDefaultAsync();
            if (toUpdate == null)
                return false;

            toUpdate.nombre = campana.nombre;
            toUpdate.descripcion = campana.descripcion;
            toUpdate.idTipoCampana = campana.idTipoCampana;
            toUpdate.activo = campana.activo;
            toUpdate.frecuencia = campana.frecuencia;
            toUpdate.fechaInicio = campana.fechaInicio;
            toUpdate.fechaFin = campana.fechaFin;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Remove(int id)
        { 
            var toDelete = await _context.Campanas.FindAsync(id);

            if (toDelete == null)
                return false;

            _context.Campanas.Remove(toDelete);
            await _context.SaveChangesAsync();
            return true;
        }
         
        //public async Task<IEnumerable<ProductoImagen>> GetImagenesByProducto(int id)
        //{ 
        //    var imagenesProductoDB = await _context.ProductoImagenes
        //                                .Include(p => p.Producto)
        //                                  //.ThenInclude(ec => ec.Categoria)
        //                                .Where(pi => pi.idproducto == id).ToListAsync();

        //    if (imagenesProductoDB != null && imagenesProductoDB.Any()) {

        //        return imagenesProductoDB.OrderBy(ip => ip.id);
        //    } 
        //    return null;
        //}
    }
}