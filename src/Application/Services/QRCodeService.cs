using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Application.Services;

namespace Application.Services
{
   public class QRCodeService : IQRCodeService
    {
        private readonly IQRCodeRepository _repo;
        private readonly IQRCodeImageService _imageService;

        public QRCodeService(IQRCodeRepository repo, 
                             IQRCodeImageService imageService) {
            _repo = repo;
            _imageService = imageService;
        }

        public Task<IEnumerable<QRCode>> GetAllAsync()
            => _repo.GetAllAsync();

        public async Task<QRCode> CreateAsync(string payload, TimeSpan? ttl)
        {
            var imageBytes = _imageService.GenerateQRCodeImage(payload);
            var qr = new QRCode(payload, ttl, imageBytes);
            await _repo.AddAsync(qr);
            return qr;
        }

        public async Task<QRCode?> GetAsync(Guid id) => await _repo.GetByIdAsync(id);

        public async Task ActivateAsync(Guid id)
        {
            var qr = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException();
            qr.Activate();
            await _repo.UpdateAsync(qr);
        }

        public async Task DeactivateAsync(Guid id)
        {
            var qr = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException();
            qr.Deactivate();
            await _repo.UpdateAsync(qr);
        }

        public async Task ConsumeAsync(Guid id)
        {
            var qr = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException();
            qr.Consume();
            await _repo.UpdateAsync(qr);
        }

        public Task<bool> Remove(Guid id)
              => _repo.Remove(id);
    }
}