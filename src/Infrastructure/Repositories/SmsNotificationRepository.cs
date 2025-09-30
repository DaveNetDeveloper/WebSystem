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
    public class SmsNotificationRepository : BaseRepository<SmsNotification>, ISmsNotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public SmsNotificationRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<PagedResult<SmsNotification>> GetByFiltersAsync(SmsNotificationFilters filters,
                                                                          IQueryOptions<SmsNotification>? options = null,
                                                                          CancellationToken cancellationToken = default)
        {
            SmsNotificationFilters _filters = ((SmsNotificationFilters)filters);

            var predicate = PredicateBuilder.New<SmsNotification>(true);

            if (_filters.Id.HasValue)
                predicate = predicate.And(u => u.id == _filters.Id.Value);

            if (!string.IsNullOrEmpty(_filters.TipoEnvioSms))
                predicate = predicate.And(u => u.tipoEnvioSms.ToLower() == _filters.TipoEnvioSms.ToLower());

            if (!string.IsNullOrEmpty(_filters.Telefono))
                predicate = predicate.And(u => u.telefono.ToLower() == _filters.Telefono.ToLower());

            if (_filters.IdUsuario.HasValue)
                predicate = predicate.And(u => u.idUsuario == _filters.IdUsuario.Value); 

            if (_filters.Activo.HasValue)
                predicate = predicate.And(u => u.activo == _filters.Activo.Value); 
             
            var query = _context.SmsNotifications.AsExpandable().
                          Where(predicate);

            int totalCount = await query.CountAsync(cancellationToken);

            query = ApplyOrdening(query, options); 
            query = ApplyPagination(query, options);
             
            var items = await query.ToListAsync(cancellationToken);

            var pageNumber = options?.Page ?? 1;
            var pageSize = options?.PageSize ?? totalCount;

            return new PagedResult<SmsNotification> {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        
        public async Task<SmsNotification?> GetByIdAsync(Guid id) =>
            await _context.SmsNotifications.FindAsync(id);
         
        public async Task<IEnumerable<SmsNotification>> GetAllAsync() =>
            await _context.SmsNotifications.ToListAsync();

        public async Task<bool> AddAsync(SmsNotification smsNotification)
        {
            var nuevaSmsNotificationDb = new SmsNotification
            {
                id = Guid.NewGuid(),
                idUsuario = smsNotification.idUsuario,
                telefono = smsNotification.telefono,
                fechaEnvio = smsNotification.fechaEnvio,
                tipoEnvioSms = smsNotification.tipoEnvioSms,
                titulo = smsNotification.titulo,
                mensaje = smsNotification.mensaje,
                activo = smsNotification.activo,
                fechaCreacion = DateTime.UtcNow
            };

            if (smsNotification.id != null)
                nuevaSmsNotificationDb.id = smsNotification.id;

            await _context.SmsNotifications.AddAsync(nuevaSmsNotificationDb);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(SmsNotification smsNotification)
        {
            var smsNotificationDb = await _context.SmsNotifications.Where(a => a.id == smsNotification.id).SingleOrDefaultAsync();
            if (smsNotificationDb == null)
                return false;

            smsNotificationDb.idUsuario = smsNotification.idUsuario;
            smsNotificationDb.tipoEnvioSms = smsNotification.tipoEnvioSms;
            smsNotificationDb.telefono = smsNotification.telefono;
            smsNotificationDb.fechaEnvio = smsNotification.fechaEnvio;
            smsNotificationDb.titulo = smsNotification.titulo;
            smsNotificationDb.mensaje = smsNotification.mensaje;
            smsNotificationDb.activo = smsNotification.activo;
            smsNotificationDb.fechaCreacion = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Remove(Guid id) {

            var smsNotification = await _context.SmsNotifications.FindAsync(id);
            if (smsNotification == null)
                return false;

            _context.SmsNotifications.Remove(smsNotification);
            await _context.SaveChangesAsync();
            return true;
        } 
    }
} 