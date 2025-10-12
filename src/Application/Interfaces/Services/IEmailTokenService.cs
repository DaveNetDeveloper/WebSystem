using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Domain.Entities;
using static Application.Services.EmailTokenService;

namespace Application.Interfaces.Services
{
    public interface IEmailTokenService : IService<EmailToken, Guid>
    { 
        DateTime GetExpirationDate(DateTime fechaCreacion);
        Task<string> GenerateEmailToken(int idUsuario, string emailAction);
        bool CheckEmailToken(string emailToken, string email);
        bool ConsumeEmailToken(string emailToken, string ip, string userAgent);
    }
}