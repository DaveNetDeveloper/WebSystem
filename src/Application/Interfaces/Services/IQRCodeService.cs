using Domain.Entities;
using static Utilities.ExporterHelper;

namespace Application.Interfaces.Services
{ 
    public interface IQRCodeService
    {
        Task<IEnumerable<QRCode>> GetAllAsync();
        Task<QRCode> CreateAsync(string payload, TimeSpan? ttl, string origen, Guid? id = null, QRCode? qrCode = null);
        Task<QRCode?> GetAsync(Guid id);
        Task<bool> UpdateAsync(QRCode qrCode);
        Task ActivateAsync(Guid id);
        Task DeactivateAsync(Guid id);
        Task<bool> ConsumeAsync(Guid id);
        Task<bool> Remove(Guid id);
        Task<byte[]> ExportarAsync(ExportFormat formato);
    }
}

