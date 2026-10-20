using MeetingManager.Domain.Entities;
using MeetingManager.Domain.RepositoryContracts;
using MeetingManager.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Infrastructure.Repositories
{
    public class RefreshTokenRepository : Repository<RefreshToken, Guid>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<RefreshToken?> FindTokenByHashAsync(string hash, CancellationToken ct = default)
        {
            return await DbSet.FirstOrDefaultAsync(t => t.TokenHash == hash, ct);
        }

        public async Task DeleteExpiredTokensAsync(CancellationToken ct = default)
        {
            await DbSet
                .Where(t => t.ExpiresAt <= DateTimeOffset.UtcNow)
                .ExecuteDeleteAsync(ct);
        }
    }
}
