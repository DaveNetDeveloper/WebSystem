using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Repositories;
using Domain.Entities; 
using Infrastructure.Persistence;
using LinqKit; 
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TransaccionRepository : BaseRepository<Transaccion>, ITransaccionRepository
    {
        private readonly ApplicationDbContext _context;
         
        public TransaccionRepository(ApplicationDbContext context) {
            _context = context;
        } 

        public async Task<IEnumerable<Transaccion>> GetByFiltersAsync(IFilters<Transaccion> filters,
                                                                      IQueryOptions<Transaccion>? options = null)
        { 
            TransaccionFilters transaccionfilters = ((TransaccionFilters)filters); 

            var predicate = PredicateBuilder.New<Transaccion>(true);

            if (transaccionfilters.Id.HasValue)
                predicate = predicate.And(u => u.id == transaccionfilters.Id.Value);

            if (!string.IsNullOrEmpty(transaccionfilters.Nombre))
                predicate = predicate.And(u => u.nombre.ToLower() == transaccionfilters.Nombre.ToLower());
            
            if (transaccionfilters.IdProducto.HasValue)
                predicate = predicate.And(u => u.idProducto == transaccionfilters.IdProducto);

            if (transaccionfilters.IdActividad.HasValue)
                predicate = predicate.And(u => u.idActividad == transaccionfilters.IdActividad);

            if (transaccionfilters.IdUsuario.HasValue)
                predicate = predicate.And(u => u.idUsuario == transaccionfilters.IdUsuario);

            if (transaccionfilters.IdTipoTransaccion != null)
                predicate = predicate.And(u => u.idTipoTransaccion == transaccionfilters.IdTipoTransaccion);

            var query = _context.Transacciones
                .AsExpandable()
                .Where(predicate);

            query = ApplyOrdening(query, options); 
            query = ApplyPagination(query, options); 
            return await query.ToListAsync();
        } 

        // TODO To Delete
        public async Task<Transaccion?> GetByIdAsync(int id) =>
            await _context.Transacciones.FindAsync(id);
          
        public async Task<IEnumerable<Transaccion>> GetAllAsync() =>
            await _context.Transacciones.ToListAsync();
        
        public async Task<bool> UpdateAsync(Transaccion transaccion)
        {
            var transaccionDb = await _context.Transacciones.Where(c => c.id == transaccion.id).SingleOrDefaultAsync();
            if (transaccionDb == null)
                return false;

            transaccionDb.nombre = transaccion.nombre;
            transaccionDb.idUsuario = transaccion.idUsuario;
            transaccionDb.idProducto = transaccion.idProducto;
            transaccionDb.idActividad = transaccion.idActividad;
            transaccionDb.puntos = transaccion.puntos;
            transaccionDb.fecha = transaccion.fecha;
            transaccionDb.idTipoTransaccion = transaccion.idTipoTransaccion;

            await _context.SaveChangesAsync();
            return true;
        } 

        public async Task<bool> AddAsync(Transaccion transaccion)
        {  
            var nuevaTransaccion = new Transaccion {
                id = (_context.Transacciones.Select(t => t.id).Max()) + 1,
                nombre = transaccion.nombre,
                idUsuario = transaccion.idUsuario,
                idProducto = transaccion.idProducto,
                idActividad = transaccion.idActividad,
                puntos = transaccion.puntos,
                fecha = DateTime.UtcNow,
                idTipoTransaccion = transaccion.idTipoTransaccion
            };

            await _context.Transacciones.AddAsync(nuevaTransaccion);
            await _context.SaveChangesAsync();
            return true;
        } 

        public async Task<bool> Remove(int id)
        {  
            var transaccionToDelete = await _context.Transacciones.FindAsync(id); 
            if (transaccionToDelete == null)
                return false;

            _context.Transacciones.Remove(transaccionToDelete);
            await _context.SaveChangesAsync();
            return true;
        } 

    }
}