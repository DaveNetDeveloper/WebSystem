using Application.Common;
using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Repositories;
using Infrastructure.Persistence;
using Domain.Entities;
 
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using LinqKit;

namespace Infrastructure.Repositories
{
    public class EmailTokenRepository : BaseRepository<EmailToken>, IEmailTokenRepository
    {
        private readonly ApplicationDbContext _context;

        public EmailTokenRepository(ApplicationDbContext context) {
            _context = context;
        }
         
        public async Task<EmailToken?> GetByIdAsync(Guid id) =>
            await _context.EmailTokens.FindAsync(id);
         
        public async Task<IEnumerable<EmailToken>> GetAllAsync() =>
            await _context.EmailTokens.ToListAsync();

        public async Task<bool> AddAsync(EmailToken emailToken)
        {
            var nuevoEmailToken = new EmailToken {
                id =Guid.NewGuid(),
                token = emailToken.token,
                userId = emailToken.userId,
                consumido = emailToken.consumido,
                emailAction = emailToken.emailAction.ToString(),
                fechaCreacion = emailToken.fechaCreacion,
                fechaExpiracion = emailToken.fechaExpiracion,
                fechaConsumido = emailToken.fechaConsumido,
                ip = emailToken.ip,
                userAgent = emailToken.userAgent
            };

            await _context.EmailTokens.AddAsync(nuevoEmailToken);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(EmailToken emailToken)
        {
            var emailTokenDb = await _context.EmailTokens.Where(a => a.id == emailToken.id).SingleOrDefaultAsync();
            if (emailTokenDb == null)
                return false;

            emailTokenDb.token = emailToken.token;
            emailTokenDb.emailAction = emailToken.emailAction;
            emailTokenDb.userId = emailToken.userId;
            emailTokenDb.fechaExpiracion = emailToken.fechaExpiracion;
            emailTokenDb.fechaCreacion = emailToken.fechaCreacion;
            emailTokenDb.fechaConsumido = emailToken.fechaConsumido;
            emailTokenDb.consumido = emailToken.consumido;
            emailTokenDb.userAgent = emailToken.userAgent;
            emailTokenDb.ip = emailToken.ip;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Remove(Guid id) {

            var emailToken = await _context.EmailTokens.FindAsync(id);

            if (emailToken == null)
                return false;

            _context.EmailTokens.Remove(emailToken);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<string> GenerateEmailToken(int idUsuario, string emailAction)
        {
            var token = Guid.NewGuid();
            var nuevoEmailToken = new EmailToken
            {
                id = Guid.NewGuid(),
                token = token,
                userId = idUsuario,
                consumido = false,
                emailAction = emailAction,
                fechaCreacion = DateTime.UtcNow,
                fechaExpiracion = DateTime.UtcNow.AddMonths(6),
                fechaConsumido = null,
                ip = null,
                userAgent = null
            };

            await _context.EmailTokens.AddAsync(nuevoEmailToken);
            await _context.SaveChangesAsync();
            
            return nuevoEmailToken.token.ToString();
        }

        public async Task<bool> CheckEmailToken(string emailToken, string email)
        {
            var user = await _context.Usuarios.SingleOrDefaultAsync(u => u.correo.ToLower() == email.ToLower());

            var emailTokenElement = await _context.EmailTokens.SingleOrDefaultAsync(a => a.token.ToString() == emailToken && a.userId == user.id);

            if (null == emailTokenElement ||
                emailTokenElement.consumido == true || 
                emailTokenElement.fechaExpiracion <= DateTime.UtcNow) {
                
                return false;
            } 
            return true;
        }

        public async Task<bool> ConsumeEmailToken(string emailToken, string ip, string userAgent)
        {
            var emailTokenElement = await _context.EmailTokens.SingleOrDefaultAsync(a => a.token.ToString() == emailToken);

            if (null != emailTokenElement && !emailTokenElement.consumido) {

                emailTokenElement.consumido = true;
                emailTokenElement.fechaConsumido = DateTime.UtcNow;

                emailTokenElement.ip = ip;
                emailTokenElement.userAgent = userAgent;

                _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}