using Domain.Entities;
using Twilio.TwiML.Voice;

namespace Application.Interfaces.Repositories;

public interface IQRCodeRepository
{
    Task<IEnumerable<QRCode>> GetAllAsync();
    Task<QRCode?> GetByIdAsync(Guid id);
    Task<bool>AddAsync(QRCode qr);
    Task<bool> UpdateAsync(QRCode qr);
    Task<bool> Remove(Guid id);
}
