using MeetingManager.Domain.Entities;

namespace MeetingManager.Domain.RepositoryContracts
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken, Guid>
    {
        Task<RefreshToken?> FindTokenByHashAsync(string hash, CancellationToken ct = default);

        Task DeleteExpiredTokensAsync(CancellationToken ct = default);
    }
}
