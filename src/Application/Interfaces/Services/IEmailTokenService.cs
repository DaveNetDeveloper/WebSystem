using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Domain.Entities;
using static Application.Services.EmailTokenService;

namespace Application.Interfaces.Services
{
    public interface IEmailTokenService : IService<EmailToken, Guid>
    { 
        public DateTime GetExpirationDate(DateTime fechaCreacion);
        public bool CheckEmailToken(string emailToken, string email);
        public bool ConsumeEmailToken(string emailToken, string ip, string userAgent);
    }
}