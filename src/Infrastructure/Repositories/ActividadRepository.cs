using Application.Common;
using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Repositories;
using Infrastructure.Persistence;
using Domain.Entities;
 
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using LinqKit;

namespace Infrastructure.Repositories
{
    public class ActividadRepository : BaseRepository<Actividad>, IActividadRepository
    {
        private readonly ApplicationDbContext _context;

        public ActividadRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<PagedResult<Actividad>> GetByFiltersAsync(ActividadFilters filters,
                                                                    IQueryOptions<Actividad>? options = null,
                                                                    CancellationToken cancellationToken = default) {
            
            ActividadFilters _filters = ((ActividadFilters)filters);

            var predicate = PredicateBuilder.New<Actividad>(true);

            if (_filters.Id.HasValue)
                predicate = predicate.And(u => u.id == _filters.Id.Value);

            if (!string.IsNullOrEmpty(_filters.Nombre))
                predicate = predicate.And(u => u.nombre.ToLower() == _filters.Nombre.ToLower());

            if (_filters.IdEntidad.HasValue)
                predicate = predicate.And(u => u.idEntidad == _filters.IdEntidad.Value);

            if (_filters.IdTipoActividad.HasValue)
                predicate = predicate.And(u => u.idTipoActividad == _filters.IdTipoActividad.Value);

            if (_filters.Activo.HasValue)
                predicate = predicate.And(u => u.activo == _filters.Activo.Value);

            if (_filters.Gratis.HasValue)
                predicate = predicate.And(u => u.gratis == _filters.Gratis.Value);
             
            var query = _context.Actividades.AsExpandable().
                          Where(predicate);

            int totalCount = await query.CountAsync(cancellationToken);

            query = ApplyOrdening(query, options); 
            query = ApplyPagination(query, options);
             
            var items = await query.ToListAsync(cancellationToken);

            var pageNumber = options?.Page ?? 1;
            var pageSize = options?.PageSize ?? totalCount;

            return new PagedResult<Actividad> {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        
        public async Task<Actividad?> GetByIdAsync(int id) =>
            await _context.Actividades.FindAsync(id);
         
        public async Task<IEnumerable<Actividad>> GetAllAsync() =>
            await _context.Actividades.ToListAsync();

        public async Task<bool> AddAsync(Actividad actividad)
        {
            var nuevaActividad = new Actividad {
                id = (_context.Actividades.Count() + 1),
                descripcion = actividad.descripcion,
                nombre = actividad.nombre,
                idEntidad = actividad.idEntidad,
                linkEvento = actividad.linkEvento,
                idTipoActividad = actividad.idTipoActividad,
                ubicación = actividad.ubicación,
                popularidad = actividad.popularidad,
                descripcionCorta = actividad.descripcionCorta,
                fechaInicio = actividad.fechaInicio,
                fechaFin = actividad.fechaFin,
                gratis = actividad.gratis,
                activo = actividad.activo,
                informacioExtra = actividad.informacioExtra,
                linkInstagram = actividad.linkInstagram,
                linkFacebook = actividad.linkFacebook,
                linkYoutube = actividad.linkYoutube
            };

            if (actividad.id != null)
                nuevaActividad.id = actividad.id;

            await _context.Actividades.AddAsync(actividad);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(Actividad actividad)
        {
            var actividadDb = await _context.Actividades.Where(a => a.id == actividad.id).SingleOrDefaultAsync();
            if (actividadDb == null)
                return false;

            actividadDb.idEntidad = actividad.idEntidad;
            actividadDb.nombre = actividad.nombre;
            actividadDb.descripcion = actividad.descripcion;
            actividadDb.linkEvento = actividad.linkEvento;
            actividadDb.idTipoActividad = actividad.idTipoActividad;
            actividadDb.ubicación = actividad.ubicación;
            actividadDb.popularidad = actividad.popularidad;
            actividadDb.descripcionCorta = actividad.descripcionCorta;
            actividadDb.fechaInicio = actividad.fechaInicio;
            actividadDb.fechaFin = actividad.fechaFin;
            actividadDb.gratis = actividad.gratis;
            actividadDb.activo = actividad.activo;
            actividadDb.informacioExtra = actividad.informacioExtra;
            actividadDb.linkInstagram = actividad.linkInstagram;
            actividadDb.linkFacebook = actividad.linkFacebook;
            actividadDb.linkYoutube = actividad.linkYoutube;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Remove(int id) {

            var actividad = await _context.Actividades.FindAsync(id);

            if (actividad == null)
                return false;

            _context.Actividades.Remove(actividad);
            await _context.SaveChangesAsync();
            return true;
        }
         
        //
        //Bindings - Table relations
        //

        public async Task<IEnumerable<Actividad>> GetActividadesByEntidad(int idEntidad)
        {
            var entidadActividades = await _context.Actividades.Where(a => a.idEntidad == idEntidad).ToListAsync();

            if (entidadActividades != null && entidadActividades.Any())  { 
                return entidadActividades.OrderBy(c => c.id);
            }
            return null;
        }

        public async Task<IEnumerable<Actividad>> GetActividadesByTipoActividad(Guid idTipoActividad)
        {
            var tipoActividades = await _context.Actividades.Where(a => a.idTipoActividad == idTipoActividad).ToListAsync();

            if (tipoActividades != null && tipoActividades.Any()) {
                return tipoActividades.OrderBy(c => c.id);
            }
            return null;
        }
         
        public async Task<IEnumerable<ActividadImagen>> GetImagenesByActividad(int idActividad)
        {
            var imagenesActividadDB = await _context.ActividadImagenes
                                        .Include(p => p.Actividad) 
                                        .Where(pi => pi.idactividad == idActividad).ToListAsync();

            if (imagenesActividadDB != null && imagenesActividadDB.Any()) { 
                return imagenesActividadDB.OrderBy(ip => ip.id);
            }
            return null;
        } 
    }
} 