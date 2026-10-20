using MeetingManager.Core.DTO;
using MeetingManager.Core.Exceptions;
using MeetingManager.Core.ServiceContracts;
using MeetingManager.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MeetingManager.WebApi.Controllers
{
    public class AuthController : DefaultControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IRefreshTokenDeleterService _refreshTokenDeleterService;

        public AuthController(IAuthService authService, IRefreshTokenDeleterService refreshTokenDeleterService)
        {
            _authService = authService;
            _refreshTokenDeleterService = refreshTokenDeleterService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<JwtTokenResponse>> Login(LoginRequest request, CancellationToken ct)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = HttpContext.Request.Headers.UserAgent.ToString();

            var response = await _authService.Login(request, ipAddress, userAgent, ct);

            Response.Cookies.Append("refreshToken", response.RefreshToken!, RefreshCookieOptions(response.RefreshTokenExpiration));
            return new JwtTokenResponse
            {
                Token = response.JwtToken,
                ExpirationDate = response.JwtTokenExpiration
            };
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(CancellationToken ct)
        {
            string? refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return NoContent();
            }

            await _refreshTokenDeleterService.DeleteTokenAsync(refreshToken, ct);

            Response.Cookies.Delete("refreshToken", RefreshCookieOptions());

            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<ActionResult<JwtTokenResponse>> Refresh(CancellationToken ct)
        {
            string? refreshToken = Request.Cookies["refreshToken"];
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = HttpContext.Request.Headers.UserAgent.ToString();
            try
            {
                var response = await _authService.RefreshTokensAsync(refreshToken, ipAddress, userAgent, ct);

                Response.Cookies.Append("refreshToken", response.RefreshToken!, RefreshCookieOptions(response.RefreshTokenExpiration));
                return new JwtTokenResponse
                {
                    Token = response.JwtToken,
                    ExpirationDate = response.JwtTokenExpiration
                };
            }
            catch (InvalidRefreshTokenException)
            {
                Response.Cookies.Delete("refreshToken", RefreshCookieOptions());
                throw;
            }
        }

        private static CookieOptions RefreshCookieOptions(DateTimeOffset? expires = null) { 
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
