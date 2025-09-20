using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Xml.Linq;

namespace Infrastructure.Repositories
{
    public class QRRepository : BaseRepository<QR>, IQRRepository
    {
        private readonly ApplicationDbContext _context;
         
        public QRRepository(ApplicationDbContext context) {
            _context = context;
        } 

        public async Task<IEnumerable<QR>> GetByFiltersAsync(IFilters<QR> filters,
                                                                  IQueryOptions<QR>? options = null)
        {
            QRFilters _filters = ((QRFilters)filters);

            var predicate = PredicateBuilder.New<QR>(true);

            if (_filters.Id.HasValue)
                predicate = predicate.And(u => u.id == _filters.Id.Value);

            if (_filters.IdProducto.HasValue)
                predicate = predicate.And(u => u.idProducto == _filters.IdProducto.Value);

            if (_filters.Consumido.HasValue)
                predicate = predicate.And(u => u.consumido == _filters.Consumido.Value);

            if (_filters.Activo.HasValue)
                predicate = predicate.And(u => u.activo == _filters.Activo.Value);

            if (_filters.Multicliente.HasValue)
                predicate = predicate.And(u => u.multicliente == _filters.Multicliente.Value);

            var query = _context.QRs
                            .AsExpandable()
                            .Where(predicate);

            query = ApplyOrdening(query, options);
            query = ApplyPagination(query, options);
            return await query.ToListAsync();
        }

        public async Task<QR?> GetByIdAsync(Guid id) =>
            await _context.QRs.FindAsync(id);

        public async Task<IEnumerable<QR>> GetAllAsync() =>
            await _context.QRs.ToListAsync();

        public async Task<bool> AddAsync(QR qr) 
        {
            var nuevoQR = new QR
            {
                id = new Guid(),
                idProducto = qr.idProducto,
                activo = qr.activo,
                multicliente = qr.multicliente,
                qrCode = qr.qrCode,
                consumido = qr.consumido,
                fechaExpiracion = qr.fechaExpiracion
            };

            await _context.QRs.AddAsync(nuevoQR);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(QR qr)
        {
            var updatedQR = await _context.QRs.Where(c => c.id == qr.id).SingleOrDefaultAsync();
            if (updatedQR == null)
                return false;

            updatedQR.idProducto = qr.idProducto;
            updatedQR.activo = qr.activo;
            updatedQR.multicliente = qr.multicliente;
            updatedQR.qrCode = qr.qrCode;
            updatedQR.consumido = qr.consumido;
            updatedQR.fechaExpiracion = qr.fechaExpiracion;
             
            await _context.SaveChangesAsync();
            //await _context.QRs.ExecuteUpdateAsync(qr);

            return true;
        }  

        public async Task<bool> Remove(Guid qrid)
        {  
            var qrToDelete = await _context.QRs.FindAsync(qrid);

            if (qrToDelete == null) 
                return false;  

            _context.QRs.Remove(qrToDelete);
            await _context.SaveChangesAsync();
            return true;  
        } 
    }
}
