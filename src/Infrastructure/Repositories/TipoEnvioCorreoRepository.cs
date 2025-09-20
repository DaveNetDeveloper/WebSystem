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

        public async Task<List<TipoEnvioCorreo>> GetAllAsync() =>
           await _context.TipoEnvioCorreos.ToListAsync();

        public async Task<TipoEnvioCorreo?> GetByNameAsync(string name) =>
           await _context.TipoEnvioCorreos.FirstOrDefaultAsync(a => a.nombre.ToLower() == name.ToLower()); 
        
    }
}
