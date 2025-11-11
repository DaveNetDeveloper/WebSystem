using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IQRCodeRepository
{
    Task<QRCode?> GetByIdAsync(Guid id);
    Task AddAsync(QRCode qr);
    Task<bool> UpdateAsync(QRCode qr);
}
