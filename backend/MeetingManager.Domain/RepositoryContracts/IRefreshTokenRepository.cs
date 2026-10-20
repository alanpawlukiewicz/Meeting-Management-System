using MeetingManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Domain.RepositoryContracts
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken, Guid>
    {
        Task<RefreshToken?> FindTokenByHashAsync(string hash, CancellationToken ct = default);

        Task DeleteExpiredTokensAsync(CancellationToken ct = default);
    }
}
