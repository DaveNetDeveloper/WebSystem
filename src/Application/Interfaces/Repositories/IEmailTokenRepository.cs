using Application.Common;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IEmailTokenRepository : IRepository<EmailToken, Guid>
    {
        public bool CheckEmailToken(string emailToken, string email);
        public bool ConsumeEmailToken(string emailToken, string ip, string userAgent);
    }
}