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
    public class ConsultaRepository : BaseRepository<Consulta>, IConsultaRepository
    {
        private readonly ApplicationDbContext _context;

        public ConsultaRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<Consulta>> GetByFiltersAsync(IFilters<Consulta> filters,
                                                                         IQueryOptions<Consulta>? options = null)
        {
            ConsultaFilters _filters = ((ConsultaFilters)filters);

            var predicate = PredicateBuilder.New<Consulta>(true);

            if (_filters.Id.HasValue)
                predicate = predicate.And(u => u.id == _filters.Id.Value);

            if (_filters.IdEntidad.HasValue)
                predicate = predicate.And(u => u.idEntidad == _filters.IdEntidad.Value);

            if (_filters.IdMotivoConsulta.HasValue)
                predicate = predicate.And(u => u.idMotivoConsulta == _filters.IdMotivoConsulta.Value);

            if (!string.IsNullOrEmpty(_filters.NombreCompleto))
                predicate = predicate.And(u => u.nombreCompleto.ToLower() == _filters.NombreCompleto.ToLower());

            if (!string.IsNullOrEmpty(_filters.Email))
                predicate = predicate.And(u => u.email.ToLower() == _filters.Email.ToLower());

            if (!string.IsNullOrEmpty(_filters.Telefono))
                predicate = predicate.And(u => u.telefono.ToLower() == _filters.Telefono.ToLower());

            var query = _context.Consultas
                            .AsExpandable()
                            .Where(predicate);

            query = ApplyOrdening(query, options);
            query = ApplyPagination(query, options);
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Consulta>> GetAllAsync() =>
           await _context.Consultas.ToListAsync();

        public async Task<Consulta?> GetByIdAsync(Guid id) =>
           await _context.Consultas.FindAsync(id);
      
        public async Task<bool> AddAsync(Consulta consulta)
        { 
            var nuevoConsulta = new Consulta {
                id = new Guid(),
                nombreCompleto = consulta.nombreCompleto,
                email = consulta.email,
                telefono = consulta.telefono,
                asunto = consulta.asunto,
                mensaje = consulta.mensaje,
                fecha = consulta.fecha,
                idEntidad = consulta.idEntidad,
                idMotivoConsulta = consulta.idMotivoConsulta
            };

            await _context.Consultas.AddAsync(nuevoConsulta);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Consulta consulta)
        {
            var updatedConsulta = await _context.Consultas.Where(r => r.id == consulta.id).SingleOrDefaultAsync();
            if (updatedConsulta == null)
                return false;

            updatedConsulta.nombreCompleto = consulta.nombreCompleto;
            updatedConsulta.email = consulta.email;
            updatedConsulta.telefono = consulta.telefono;
            updatedConsulta.asunto = consulta.asunto;
            updatedConsulta.mensaje = consulta.mensaje;
            updatedConsulta.fecha = consulta.fecha;
            updatedConsulta.idEntidad = consulta.idEntidad;
            updatedConsulta.idMotivoConsulta = consulta.idMotivoConsulta;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Remove(Guid id)
        {
            var consultaToDelete = await _context.Consultas.FindAsync(id); 
            if (consultaToDelete == null)
                return false;

            _context.Consultas.Remove(consultaToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}