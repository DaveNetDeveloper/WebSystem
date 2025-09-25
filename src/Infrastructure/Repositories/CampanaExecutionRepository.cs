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
    public class CampanaExecutionRepository : BaseRepository<CampanaExecution>, 
                                              ICampanaExecutionRepository
    {
        private readonly ApplicationDbContext _context;
         
        public CampanaExecutionRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<CampanaExecution>> GetByFiltersAsync(IFilters<CampanaExecution> filters,
                                                                           IQueryOptions<CampanaExecution>? options = null)
        {
            CampanaExecutionFilters _filters = ((CampanaExecutionFilters)filters);

            var predicate = PredicateBuilder.New<CampanaExecution>(true);

            if (_filters.Id.HasValue)
                predicate = predicate.And(u => u.id == _filters.Id.Value);

            if (_filters.IdCampana.HasValue)
                predicate = predicate.And(u => u.idCampana == _filters.IdCampana.Value);
             
            if (!string.IsNullOrEmpty(_filters.Estado))
                predicate = predicate.And(u => u.estado.ToLower() == _filters.Estado.ToLower());

            var query = _context.CampanaExecutions
                            .AsExpandable()
                            .Where(predicate);

            query = ApplyOrdening(query, options);
            query = ApplyPagination(query, options);
            return await query.ToListAsync();
        }

        public async Task<CampanaExecution?> GetByIdAsync(Guid id) =>
            await _context.CampanaExecutions.FindAsync(id);
 
        public async Task<IEnumerable<CampanaExecution>> GetAllAsync() =>
            await _context.CampanaExecutions.ToListAsync();
        
        public async Task<bool> AddAsync(CampanaExecution campanaExecution)
        {
            var nuevaCampanaExecution = new CampanaExecution {
                id = Guid.NewGuid(),
                idCampana = campanaExecution.idCampana,
                estado = campanaExecution.estado,
                fechaEjecucion = campanaExecution.fechaEjecucion
            };

            await _context.CampanaExecutions.AddAsync(nuevaCampanaExecution);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(CampanaExecution campanaExecution)
        {
            var toUpdate = await _context.CampanaExecutions.Where(c => c.id == campanaExecution.id).SingleOrDefaultAsync();
            if (toUpdate == null)
                return false; 

            toUpdate.idCampana = campanaExecution.idCampana;
            toUpdate.fechaEjecucion = campanaExecution.fechaEjecucion;
            toUpdate.estado = campanaExecution.estado; 

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Remove(Guid id)
        { 
            var toDelete = await _context.CampanaExecutions.FindAsync(id);

            if (toDelete == null)
                return false;

            _context.CampanaExecutions.Remove(toDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}