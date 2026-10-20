using MeetingManager.Core.DTO;
using MeetingManager.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MeetingManager.WebApi.Controllers
{
    public class AuthController : DefaultControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<JwtTokenResponse>> Login(LoginRequest request, CancellationToken ct)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = HttpContext.Request.Headers.UserAgent.ToString();

            var response = await _authService.Login(request, ipAddress, userAgent, ct);

            Response.Cookies.Append("refreshToken", response.RefreshToken!, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = response.RefreshTokenExpiration,
                Path = "/api/auth",
            });
            return new JwtTokenResponse
            {
                Token = response.JwtToken,
                ExpirationDate = response.JwtTokenExpiration
            };
        }
    }
}
