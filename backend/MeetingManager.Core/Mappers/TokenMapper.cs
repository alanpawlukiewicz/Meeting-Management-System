using MeetingManager.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;

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
