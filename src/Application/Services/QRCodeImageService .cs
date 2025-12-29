using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class QRCodeImageService : IQRCodeImageService
    {
        private readonly IQRCodeImageRepository _repo;
        
        public QRCodeImageService(IQRCodeImageRepository repo)
        {
            _repo = repo;
        }

        public byte[] GenerateQRCodeImage(string content)
        {
            return _repo.GenerateQRCodeImage(content);
        }
    }
}