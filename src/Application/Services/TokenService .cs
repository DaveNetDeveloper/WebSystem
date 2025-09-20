using Application.Interfaces;
using Domain.Entities;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;

namespace Application.Services
{
    public class TokenService : ITokenService
    {

        private readonly ITokenRepository _tokenRepository;

        public TokenService(ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        } 

        public bool ValidarToken(string token)
        {

            return _tokenRepository.ValidarToken(token);

        } 
        public async Task<Usuario?> ValidarTokenAsync(string token)
        {
            return await _tokenRepository.GetUsuarioByTokenAsync(token);
        }

        public async Task GuardarTokenAsync(int usuarioId, string token)
        {
            await _tokenRepository.SaveTokenAsync(usuarioId, token);
        }

        public async Task EliminarTokenAsync(string token)
        {
            await _tokenRepository.RemoveTokenAsync(token);
        }
    }
}