using MeetingManager.Core.DTO;
using MeetingManager.Core.Mappers;
using MeetingManager.Core.ServiceContracts;
using MeetingManager.Domain.RepositoryContracts;

namespace MeetingManager.Core.Services
{
    public class RefreshTokenAdderService : IRefreshTokenAdderService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ITokenHasher _tokenHasher;

        public RefreshTokenAdderService(IRefreshTokenRepository refreshTokenRepository, ITokenHasher tokenHasher)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _tokenHasher = tokenHasher;
        }

        public async Task AddTokenAsync(RefreshTokenAddRequest request, CancellationToken ct = default)
        {
            var hash = _tokenHasher.Hash(request.Token);
            _refreshTokenRepository.Add(request.ToRefreshToken(hash));
            await _refreshTokenRepository.SaveChangesAsync(ct);
        }
    }
}
