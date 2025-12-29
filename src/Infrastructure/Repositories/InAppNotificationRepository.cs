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
    public class InAppNotificationRepository : BaseRepository<InAppNotification>, 
                                               IInAppNotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public InAppNotificationRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<PagedResult<InAppNotification>> GetByFiltersAsync(InAppNotificationFilters filters,
                                                                            IQueryOptions<InAppNotification>? options = null,
                                                                            CancellationToken cancellationToken = default)
        {
            InAppNotificationFilters _filters = ((InAppNotificationFilters)filters);

            var predicate = PredicateBuilder.New<InAppNotification>(true);

            if (_filters.Id.HasValue)
                predicate = predicate.And(u => u.id == _filters.Id.Value);

            if (!string.IsNullOrEmpty(_filters.TipoEnvioInApp))
                predicate = predicate.And(u => u.tipoEnvioInApp.ToLower() == _filters.TipoEnvioInApp.ToLower());
            
            if (_filters.IdUsuario.HasValue)

                predicate = predicate.And(u => u.idUsuario == _filters.IdUsuario.Value); 

            if (_filters.Activo.HasValue)
                predicate = predicate.And(u => u.activo == _filters.Activo.Value); 
             
            var query = _context.InAppNotifications.AsExpandable().
                          Where(predicate);

            int totalCount = await query.CountAsync(cancellationToken);

            query = ApplyOrdening(query, options); 
            query = ApplyPagination(query, options);
             
            var items = await query.ToListAsync(cancellationToken);

            var pageNumber = options?.Page ?? 1;
            var pageSize = options?.PageSize ?? totalCount;

            return new PagedResult<InAppNotification> {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        
        public async Task<InAppNotification?> GetByIdAsync(Guid id) =>
            await _context.InAppNotifications.FindAsync(id);
         
        public async Task<IEnumerable<InAppNotification>> GetAllAsync() =>
            await _context.InAppNotifications.ToListAsync();

        public async Task<bool> AddAsync(InAppNotification inAppNotification)
        {
            var nuevaInAppNotificationDb = new InAppNotification {
                id = Guid.NewGuid(),
                idUsuario = inAppNotification.idUsuario,
                tipoEnvioInApp = inAppNotification.tipoEnvioInApp,
                titulo = inAppNotification.titulo,
                mensaje = inAppNotification.mensaje,
                activo = inAppNotification.activo,
                fechaCreacion = DateTime.UtcNow
            };

            if (inAppNotification.id != null)
                nuevaInAppNotificationDb.id = inAppNotification.id;

            await _context.InAppNotifications.AddAsync(nuevaInAppNotificationDb);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(InAppNotification inAppNotification)
        {
            var inAppNotificationDb = await _context.InAppNotifications.Where(a => a.id == inAppNotification.id).SingleOrDefaultAsync();
            if (inAppNotificationDb == null)
                return false;

            inAppNotificationDb.idUsuario = inAppNotification.idUsuario;
            inAppNotificationDb.tipoEnvioInApp = inAppNotification.tipoEnvioInApp;
            inAppNotificationDb.titulo = inAppNotification.titulo;
            inAppNotificationDb.mensaje = inAppNotification.mensaje;
            inAppNotificationDb.activo = inAppNotification.activo;
            //inAppNotificationDb.fechaCreacion = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Remove(Guid id) {

            var inAppNotification = await _context.InAppNotifications.FindAsync(id);
            if (inAppNotification == null)
                return false;

            _context.InAppNotifications.Remove(inAppNotification);
            await _context.SaveChangesAsync();
            return true;
        }

        //
        //
        //
        public async Task<IEnumerable<string>> ObtenerTiposEnvioInApp() =>
          await _context.InAppNotifications
                        .Select(s => s.tipoEnvioInApp)
                        .Distinct()
                        .ToListAsync();
    }
} 