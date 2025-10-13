using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using System.Linq;
using LinqKit;

namespace Infrastructure.Repositories
{
    public class LogRepository : BaseRepository<Log>, ILogRepository
    {
        private readonly ApplicationDbContext _context;

        public LogRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<Log>> GetByFiltersAsync(IFilters<Log> filters,
                                                              IQueryOptions<Log>? options = null) {
            LogFilters _filters = ((LogFilters)filters);

            var predicate = PredicateBuilder.New<Log>(true);

            if (_filters.Id.HasValue)
                predicate = predicate.And(u => u.id == _filters.Id.Value);

            if (_filters.IdUsuario.HasValue)
                predicate = predicate.And(u => u.idUsuario == _filters.IdUsuario.Value);

            if (!string.IsNullOrEmpty(_filters.TipoLog))
                predicate = predicate.And(u => u.tipoLog.ToLower() == _filters.TipoLog.ToLower());

            if (!string.IsNullOrEmpty(_filters.Proceso))
                predicate = predicate.And(u => u.proceso.ToLower() == _filters.Proceso.ToLower());

            if (_filters.Fecha_Ini.HasValue)
                predicate = predicate.And(u => u.fecha >= _filters.Fecha_Ini.Value);

            if (_filters.Fecha_Fin.HasValue)
                predicate = predicate.And(u => u.fecha <= _filters.Fecha_Fin.Value);

            var query = _context.Logs
                            .AsExpandable()
                            .Where(predicate);

            query = ApplyOrdening(query, options);
            query = ApplyPagination(query, options);
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Log>> GetAllAsync() =>
           await _context.Logs.ToListAsync();

        public async Task<Log?> GetByIdAsync(Guid id) =>
           await _context.Logs.FindAsync(id);
      
        public async Task<bool> AddAsync(Log log)
        { 
            var nuevoLog = new Log
            {
                id = Guid.NewGuid(),
                idUsuario = log.idUsuario,
                tipoLog = log.tipoLog,
                detalle = log.detalle,
                proceso = log.proceso,
                titulo = log.titulo,
                fecha = log.fecha
            };

            await _context.Logs.AddAsync(nuevoLog);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Log log)
        {
            var updatedLog = await _context.Logs.Where(r => r.id == log.id).SingleOrDefaultAsync();
            if (updatedLog == null)
                return false;

            updatedLog.idUsuario = log.idUsuario;
            updatedLog.tipoLog = log.tipoLog;
            updatedLog.proceso = log.proceso;
            updatedLog.titulo = log.titulo;
            updatedLog.detalle = log.detalle;
            updatedLog.fecha = log.fecha;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Remove(Guid id)
        {
            var consultaToDelete = await _context.Logs.FindAsync(id); 
            if (consultaToDelete == null)
                return false;

            _context.Logs.Remove(consultaToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}