using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;

using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository 
    {
        private readonly ApplicationDbContext _context;

        public RefreshTokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<RefreshToken?> GetByTokenHashAsync(string refreshTokenHashed)
        {
            return _context.RefreshTokens.FirstOrDefaultAsync(x => x.refreshToken == refreshTokenHashed && x.isRevoked == false);

        }

        public async Task<bool> AddAsync(RefreshToken refreshToken)
        {
            refreshToken.id = _context.RefreshTokens.Count() + 1;
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RevokeRefreshTokenAsync(string refreshTokenHashed)
        {
            var refreshTokenToRevoke = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.refreshToken == refreshTokenHashed);
            if (refreshTokenToRevoke == null) return false;

            refreshTokenToRevoke.isRevoked = true;
            refreshTokenToRevoke.revokedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}