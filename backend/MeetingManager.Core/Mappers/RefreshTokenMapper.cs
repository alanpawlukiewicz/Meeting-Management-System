using MeetingManager.Core.DTO;
using MeetingManager.Domain.Entities;

namespace MeetingManager.Core.Mappers
{
    public static class RefreshTokenMapper
    {
        public static RefreshToken ToRefreshToken(this RefreshTokenAddRequest request, string tokenHash)
        {
            return new RefreshToken
            {
                Id = Guid.NewGuid(),
                ExpiresAt = request.ExpiresAt,
                IpAddress = request.IpAddress,
                TokenHash = tokenHash,
                UserAgent = request.UserAgent,
                UserId = request.UserId
            };
        }
    }
}
