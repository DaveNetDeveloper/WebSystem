using Domain.Entities; 
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly ApplicationDbContext _context;

        public TokenRepository(ApplicationDbContext context) {
            _context = context;
        } 
        public bool ValidarToken(string token)
        {
            if (!string.IsNullOrWhiteSpace(token)) { 
                var usuario = _context.Usuarios
                    .SingleOrDefault(u => u.token == token.Replace("Bearer ", string.Empty));

                if (usuario == null) return false;

                var expiracionToken = usuario.expiracionToken;
                if (expiracionToken == null || expiracionToken > DateTime.UtcNow) {
                    return true;
                }
            }
            return false;
        }
         
        public async Task<Usuario?> GetUsuarioByTokenAsync(string token)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.token == token);
        }

        public async Task SaveTokenAsync(int usuarioId, string token)
        {
            var usuario = await _context.Usuarios.FindAsync(usuarioId);
            if (usuario != null) {
                usuario.token = token;
                await _context.SaveChangesAsync();
            }
        }
        public async Task RemoveTokenAsync(string token)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.token == token);

            if (usuario != null) {
                usuario.token = null;
                usuario.expiracionToken = null;
                await _context.SaveChangesAsync();
            }
        }
    }
}
