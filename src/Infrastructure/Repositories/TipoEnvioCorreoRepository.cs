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
    public class TipoEnvioCorreoRepository : BaseRepository<TipoEnvioCorreo>, ITipoEnvioCorreoRepository
    {
        private readonly ApplicationDbContext _context;

        public TipoEnvioCorreoRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<TipoEnvioCorreo>> GetByFiltersAsync(IFilters<TipoEnvioCorreo> filters,
                                                                          IQueryOptions<TipoEnvioCorreo>? options = null)
        {
            TipoEnvioCorreoFilters _filters = ((TipoEnvioCorreoFilters)filters); 

            var predicate = PredicateBuilder.New<TipoEnvioCorreo>(true);

            if (_filters.Id.HasValue)
                predicate = predicate.And(u => u.id == _filters.Id.Value);

            if (!string.IsNullOrEmpty(_filters.Nombre))
                predicate = predicate.And(u => u.nombre.ToLower() == _filters.Nombre.ToLower()); 

            if (_filters.Activo.HasValue)
                predicate = predicate.And(u => u.activo == _filters.Activo.Value);

            var query = _context.TipoEnvioCorreos
                            .AsExpandable()
                            .Where(predicate);

            query = ApplyOrdening(query, options); 
            query = ApplyPagination(query, options); 
            return await query.ToListAsync();
        }

        public async Task<TipoEnvioCorreo?> GetByIdAsync(Guid id) =>
            await _context.TipoEnvioCorreos.FindAsync(id);

        public async Task<IEnumerable<TipoEnvioCorreo>> GetAllAsync() =>
           await _context.TipoEnvioCorreos.OrderBy(t => t.nombre).ToListAsync();

        public async Task<bool> AddAsync(TipoEnvioCorreo tipoEnvioCorreo)
        {
            var toCreate = new TipoEnvioCorreo
            {
                id = new Guid(),
                nombre = tipoEnvioCorreo.nombre,
                descripcion = tipoEnvioCorreo.descripcion,
                activo = tipoEnvioCorreo.activo,
                asunto = tipoEnvioCorreo.asunto,
                cuerpo = tipoEnvioCorreo.cuerpo
            };

            await _context.TipoEnvioCorreos.AddAsync(toCreate);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(TipoEnvioCorreo tipoEnvioCorreo)
        {
            var tipoEnvioCorreoDB = await _context.TipoEnvioCorreos.Where(c => c.id == tipoEnvioCorreo.id).SingleOrDefaultAsync();
            if (tipoEnvioCorreoDB == null)
                return false;

            tipoEnvioCorreoDB.nombre = tipoEnvioCorreo.nombre;
            tipoEnvioCorreoDB.descripcion = tipoEnvioCorreo.descripcion;
            tipoEnvioCorreoDB.activo = tipoEnvioCorreo.activo;
            tipoEnvioCorreoDB.asunto = tipoEnvioCorreo.asunto;
            tipoEnvioCorreoDB.cuerpo = tipoEnvioCorreo.cuerpo;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Remove(Guid id)
        {
            var toDelete = await _context.TipoEnvioCorreos.FindAsync(id);

            if (toDelete == null)
                return false;

            _context.TipoEnvioCorreos.Remove(toDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
