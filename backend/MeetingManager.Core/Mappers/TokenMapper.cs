using MeetingManager.Core.DTO;

namespace MeetingManager.Core.Mappers
{
    public static class TokenMapper
    {
        public static JwtTokenResponse ToJwtTokenResponse(this AuthResponse response)
        {
            return new JwtTokenResponse
            {
                Token = response.JwtToken,
                ExpirationDate = response.JwtTokenExpiration
            };
        }
    }
}
