using Application.DTOs.Filters;
using Application.Interfaces;
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

namespace Application.Services
{
    public class EmailTokenService : IEmailTokenService
    {
        private readonly IEmailTokenRepository _repo;

        public EmailTokenService(IEmailTokenRepository repo) {
            _repo = repo;
        }

        public Task<IEnumerable<EmailToken>> GetAllAsync()
            => _repo.GetAllAsync();

        public Task<EmailToken?> GetByIdAsync(Guid id)
           => _repo.GetByIdAsync(id);  
        public Task<bool> AddAsync(EmailToken emailToken)
            => _repo.AddAsync(emailToken);

        public Task<bool> UpdateAsync(EmailToken emailToken)
             => _repo.UpdateAsync(emailToken);

        public Task<bool> Remove(Guid id)
              => _repo.Remove(id); 

        public DateTime GetExpirationDate(DateTime fechaCreacion) {
            return fechaCreacion.AddDays(3);
        }

        public Task<string> GenerateEmailToken(int idUsuario, string emailAction)
            => _repo.GenerateEmailToken(idUsuario, emailAction);

        public bool CheckEmailToken(string emailToken, string email)
            => _repo.CheckEmailToken(emailToken, email);

        public bool ConsumeEmailToken(string emailToken, string ip, string userAgent)
            => _repo.ConsumeEmailToken(emailToken, ip, userAgent); 
    }
}