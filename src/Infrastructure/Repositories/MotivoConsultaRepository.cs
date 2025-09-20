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
    public class MotivoConsultaRepository : BaseRepository<MotivoConsulta>, IMotivoConsultaRepository
    {
        private readonly ApplicationDbContext _context;

        public MotivoConsultaRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<MotivoConsulta>> GetByFiltersAsync(IFilters<MotivoConsulta> filters,
                                                                  IQueryOptions<MotivoConsulta>? options = null)
        {
            MotivoConsultaFilters _filters = ((MotivoConsultaFilters)filters);

            var predicate = PredicateBuilder.New<MotivoConsulta>(true);

            if (_filters.Id.HasValue)
                predicate = predicate.And(u => u.id == _filters.Id.Value);

            if (_filters.IdTipoEntidad.HasValue)
                predicate = predicate.And(u => u.idtipoentidad == _filters.IdTipoEntidad.Value); 

            if (!string.IsNullOrEmpty(_filters.Nombre))
                predicate = predicate.And(u => u.nombre.ToLower() == _filters.Nombre.ToLower());

            var query = _context.MotivosConsultas
                            .AsExpandable()
                            .Where(predicate);

            query = ApplyOrdening(query, options);
            query = ApplyPagination(query, options);
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<MotivoConsulta>> GetAllAsync() =>
           await _context.MotivosConsultas.ToListAsync();

        public async Task<MotivoConsulta?> GetByIdAsync(Guid id) =>
           await _context.MotivosConsultas.FindAsync(id);

      
        public async Task<bool> AddAsync(MotivoConsulta motivoConsulta)
        { 
            var nuevoMotivoConsulta = new MotivoConsulta {
                id = new Guid(),
                nombre = motivoConsulta.nombre,
                descripcion = motivoConsulta.descripcion,
                idtipoentidad = motivoConsulta.idtipoentidad
            };

            await _context.MotivosConsultas.AddAsync(nuevoMotivoConsulta);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(MotivoConsulta motivoConsulta)
        {
            var updatedMotivoConsulta = await _context.MotivosConsultas.Where(r => r.id == motivoConsulta.id).SingleOrDefaultAsync();
            if (updatedMotivoConsulta == null)
                return false;

            updatedMotivoConsulta.nombre = motivoConsulta.nombre;
            updatedMotivoConsulta.descripcion = motivoConsulta.descripcion;
            updatedMotivoConsulta.idtipoentidad = motivoConsulta.idtipoentidad;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Remove(Guid id)
        {
            var motivoConsultaToDelete = await _context.MotivosConsultas.FindAsync(id); 
            if (motivoConsultaToDelete == null)
                return false;

            _context.MotivosConsultas.Remove(motivoConsultaToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}