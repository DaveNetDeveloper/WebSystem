//using Application.Interfaces.Common;
//using Domain.Entities;

namespace Application.Interfaces.Services
{ 
    public interface IQRCodeImageService
    {
        byte[] GenerateQRCodeImage(string content);
    }
}
