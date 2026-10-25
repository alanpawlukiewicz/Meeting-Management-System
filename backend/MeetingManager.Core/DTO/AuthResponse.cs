namespace MeetingManager.Core.DTO
{
    public class AuthResponse
    {
        public string JwtToken { get; set; } = string.Empty;
        public DateTimeOffset JwtTokenExpiration { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTimeOffset RefreshTokenExpiration { get; set; }
    }
}
