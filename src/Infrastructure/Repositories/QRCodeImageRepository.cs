using Application.Interfaces;
using Application.Interfaces.Repositories;
using QRCoder;

namespace Infrastructure.Repositories
{
        public class QRCodeImageRepository : IQRCodeImageRepository
    {
        public byte[] GenerateQRCodeImage(string content)
        {
            using var generator = new QRCodeGenerator();
            using var data = generator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new PngByteQRCode(data);
            return qrCode.GetGraphic(20);
        }
    }
}