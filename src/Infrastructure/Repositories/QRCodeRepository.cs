using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Services;
using Infrastructure.Persistence;
using Domain.Entities;

using Microsoft.EntityFrameworkCore;
using System;

using QRCoder;
using QRCode = Domain.Entities.QRCode;

namespace Infrastructure.Repositories
{
    public class QRCodeRepository : IQRCodeRepository
    {
        private readonly ApplicationDbContext _context;

        public QRCodeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<QRCode>> GetAllAsync() =>
            await _context.QRCodes.ToListAsync();

        public async Task<QRCode?> GetByIdAsync(Guid id) =>
            await _context.QRCodes.FirstOrDefaultAsync(q => q.id == id);

        public async Task<bool> AddAsync(QRCode qr)
        {
            _context.QRCodes.Add(qr);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(QRCode qr)
        {
            var qrToUpdate = await _context.QRCodes.Where(c => c.id == qr.id).SingleOrDefaultAsync();
            if (qrToUpdate == null)
                return false;

            qrToUpdate.id = qr.id;
            qrToUpdate.token = qr.token;
            qrToUpdate.imagen = qr.imagen;
            qrToUpdate.fechaCreacion = qr.fechaCreacion;
            qrToUpdate.fechaExpiracion = qr.fechaExpiracion;
            qrToUpdate.payload = qr.payload;
            qrToUpdate.status = qr.status;
             
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Remove(Guid id)
        {
            var qrToDelete = await _context.QRCodes.FindAsync(id);

            if (qrToDelete == null)
                return false;

            _context.QRCodes.Remove(qrToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}