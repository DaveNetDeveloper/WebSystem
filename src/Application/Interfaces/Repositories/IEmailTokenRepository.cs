using Application.Common;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IEmailTokenRepository : IRepository<EmailToken, Guid>
    {
        Task<string> GenerateEmailToken(int idUsuariol, string emailAction);
        bool CheckEmailToken(string emailToken, string email);
        bool ConsumeEmailToken(string emailToken, string ip, string userAgent);
    }
}