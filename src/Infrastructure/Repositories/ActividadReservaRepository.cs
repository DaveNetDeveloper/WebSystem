using Application.DTOs.Filters;
using Application.DTOs.Responses;
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
    public class ActividadReservaRepository : BaseRepository<ActividadReserva>,
                                              IActividadReservaRepository
    {
        private readonly ApplicationDbContext _context;

        public ActividadReservaRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<ActividadReserva>> GetAllAsync() =>
            await _context.ActividadReservas.ToListAsync();

        public async Task<IEnumerable<ActividadReserva>> GetByFiltersAsync(IFilters<ActividadReserva> filters,
                                                                           IQueryOptions<ActividadReserva>? options = null)
        {
            ActividadReservaFilters _filters = ((ActividadReservaFilters)filters);

            var predicate = PredicateBuilder.New<ActividadReserva>(true);

            if (_filters.IdReserva.HasValue)
                predicate = predicate.And(u => u.idReserva == _filters.IdReserva.Value);

            if (!string.IsNullOrEmpty(_filters.CodigoReserva))
                predicate = predicate.And(u => u.codigoReserva.ToLower() == _filters.CodigoReserva.ToLower());

            if (!string.IsNullOrEmpty(_filters.Estado))
                predicate = predicate.And(u => u.estado.ToLower() == _filters.Estado.ToLower());

            if (_filters.IdUsuario.HasValue)
                predicate = predicate.And(u => u.idUsuario == _filters.IdUsuario.Value);

            if (_filters.IdActividad.HasValue)
                predicate = predicate.And(u => u.idActividad == _filters.IdActividad.Value);

            var query = _context.ActividadReservas
                            .AsExpandable()
                            .Where(predicate);

            query = ApplyOrdening(query, options);
            query = ApplyPagination(query, options);
            return await query.ToListAsync();
        }

        public async Task<ActividadReserva?> GetByIdAsync(Guid id) =>
            await _context.ActividadReservas.FindAsync(id);

        public async Task<bool> AddAsync(ActividadReserva actividadReserva)
        {
            var nuevaReserva = new ActividadReserva
            {
                idReserva = Guid.NewGuid(),
                codigoReserva = actividadReserva.codigoReserva,
                idActividad = actividadReserva.idActividad,
                idUsuario = actividadReserva.idUsuario,
                fechaReserva = actividadReserva.fechaReserva,
                fechaActividad = actividadReserva.fechaActividad,
                estado = actividadReserva.estado,
                fechaValidacion = actividadReserva.fechaValidacion

            }; 
            await _context.ActividadReservas.AddAsync(nuevaReserva);
            await _context.SaveChangesAsync();
            return true;
        } 

        public async Task<bool> UpdateAsync(ActividadReserva actividadReserva)
        {
            var updatedReserva = await _context.ActividadReservas
                                                .Where(r => r.idReserva == actividadReserva.idReserva)
                                                .SingleOrDefaultAsync();
            if (updatedReserva == null)
                return false;

            updatedReserva.codigoReserva = actividadReserva.codigoReserva;
            updatedReserva.idActividad = actividadReserva.idActividad;
            updatedReserva.idUsuario = actividadReserva.idUsuario;
            updatedReserva.fechaReserva = actividadReserva.fechaReserva;
            updatedReserva.fechaActividad = actividadReserva.fechaActividad;
            updatedReserva.estado = actividadReserva.estado;
            updatedReserva.fechaValidacion = actividadReserva.fechaValidacion;

            await _context.SaveChangesAsync();
            return true;
        } 

        public async Task<bool> Remove(Guid id)
        { 
            var reservaToDelete = await _context.ActividadReservas.FindAsync(id);

            if (reservaToDelete == null)
                return false;

            _context.ActividadReservas.Remove(reservaToDelete);
            await _context.SaveChangesAsync();
            return true;
        } 
    }
}