using MeetingManager.Core.ServiceContracts;
using MeetingManager.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Core.Services
{
    public class RefreshTokenDeleterService : IRefreshTokenDeleterService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ITokenHasher _tokenHasher;

        public RefreshTokenDeleterService(IRefreshTokenRepository refreshTokenRepository, ITokenHasher tokenHasher)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _tokenHasher = tokenHasher;
        }

        public async Task DeleteTokenAsync(string token, CancellationToken ct = default)
        {
            string hashedToken = _tokenHasher.Hash(token);

            var tokenRecord = await _refreshTokenRepository.FindTokenByHashAsync(hashedToken, ct);

            if (tokenRecord != null)
            {
                _refreshTokenRepository.Remove(tokenRecord);
                await _refreshTokenRepository.SaveChangesAsync(ct);
            }
        }
    }
}
