using Domain.Entities;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Common;

namespace Application.Interfaces.Repositories
{
    public interface IRefreshTokenRepository 
    {
        Task<bool> AddAsync(RefreshToken refreshToken); 
        Task<RefreshToken?> GetByTokenHashAsync(string refreshToken); // si usas hash s
        Task<bool> RevokeRefreshTokenAsync(string refreshToken);
         
    }
}