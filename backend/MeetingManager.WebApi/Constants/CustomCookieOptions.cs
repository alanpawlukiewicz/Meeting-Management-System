namespace MeetingManager.WebApi.Constants
{
    public static class CustomCookieOptions
    {
        public static CookieOptions RefreshCookieOptions(DateTimeOffset? expires = null)
        {
            return new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Path = "/api/auth",
                Expires = expires
            };
        }
    }
}
