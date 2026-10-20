using MeetingManager.Core.DTO;
using MeetingManager.Core.Mappers;
using MeetingManager.Core.ServiceContracts;
using MeetingManager.Domain.Entities;
using MeetingManager.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Core.Services
{
    public class RefreshTokenRotationService : IRefreshTokenRotationService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ITokenHasher _tokenHasher;

        public RefreshTokenRotationService(IRefreshTokenRepository refreshTokenRepository, ITokenHasher tokenHasher)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _tokenHasher = tokenHasher;
        }

        public async Task RotateAsync(RefreshToken oldToken, RefreshTokenAddRequest newToken, CancellationToken ct = default)
        {
            var hash = _tokenHasher.Hash(newToken.Token);
            _refreshTokenRepository.Remove(oldToken);
            _refreshTokenRepository.Add(newToken.ToRefreshToken(hash));

            await _refreshTokenRepository.SaveChangesAsync(ct);
        }
    }
}
