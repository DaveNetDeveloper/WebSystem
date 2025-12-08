using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Services;
using Domain.Entities;
using System.Collections;
using static Application.Services.DataQueryService;
using static Utilities.ExporterHelper;

namespace Application.Services
{
   public class QRCodeService : IQRCodeService
    {
        private readonly IQRCodeRepository _repo;
        private readonly IQRCodeImageService _imageService;
        private readonly IExcelExporter _excelExporter;
        private readonly IExporter _pdfExporter;

        public QRCodeService(IQRCodeRepository repo, 
                             IQRCodeImageService imageService,
                              IExcelExporter excelExporter,
                              IExporter pdfExporter) {
            _repo = repo;
            _imageService = imageService;
            _excelExporter = excelExporter;
            _pdfExporter = pdfExporter;
        }

        public byte[] ExportDynamic(IEnumerable data, Type entityType)
        {
            return null;
        }

        public byte[] Export<T>(IEnumerable<T> data, string sheetName)
        {
            return null;
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
  
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataQueryType"></param>
        /// <returns></returns>
        /// 
        public async Task<byte[]> ExportarAsync(ExportFormat formato) // TODO por implementar en todas las entidades exportables
        {
            Type entityType = typeof(QRCode);
            IEnumerable queryResult = await GetAllAsync();

            byte[] excelBytes = null;
            switch (formato)
            {
                case ExportFormat.Excel:
                    excelBytes = _excelExporter.ExportToExcelDynamic(queryResult, entityType);
                    break;
                case ExportFormat.Pdf:
                    excelBytes = _pdfExporter.ExportDynamic(queryResult, entityType);
                    break;
            }
            return excelBytes;
        }

        public async Task<byte[]> ExportarAsync(DataQueryType dataQueryType, ExportFormat formato)
        {
            return null;
        }

    }
}