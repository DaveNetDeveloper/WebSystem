using Domain.Entities;
using static Utilities.ExporterHelper;

namespace Application.Interfaces.Services
{ 
    public interface IQRCodeService
    {
        Task<IEnumerable<QRCode>> GetAllAsync();
        Task<QRCode> CreateAsync(string payload, TimeSpan? ttl);
        Task<QRCode?> GetAsync(Guid id);
        Task ActivateAsync(Guid id);
        Task DeactivateAsync(Guid id);
        Task ConsumeAsync(Guid id);
        Task<bool> Remove(Guid id);
        Task<byte[]> ExportarAsync(ExportFormat formato);
    }
}
