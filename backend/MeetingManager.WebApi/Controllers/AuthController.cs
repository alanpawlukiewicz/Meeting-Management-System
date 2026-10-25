using MeetingManager.Core.DTO;
using MeetingManager.Core.Exceptions;
using MeetingManager.Core.Mappers;
using MeetingManager.Core.ServiceContracts;
using MeetingManager.Core.ServiceContracts.UserServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeetingManager.WebApi.Controllers
{
    public class AuthController : DefaultControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IRefreshTokenDeleterService _refreshTokenDeleterService;
        private readonly IUserRegistrationService _userRegistrationService;

        public AuthController(IAuthService authService, IRefreshTokenDeleterService refreshTokenDeleterService, IUserRegistrationService userRegistrationService)
        {
            _authService = authService;
            _refreshTokenDeleterService = refreshTokenDeleterService;
            _userRegistrationService = userRegistrationService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<JwtTokenResponse>> RegisterUser(UserRegisterRequest request, CancellationToken ct)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = HttpContext.Request.Headers.UserAgent.ToString();

            var response = await _userRegistrationService.RegisterUserAsync(request, ipAddress, userAgent, ct);

            Response.Cookies.Append("refreshToken", response.RefreshToken!, RefreshCookieOptions(response.RefreshTokenExpiration));
            return response.ToJwtTokenResponse();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<JwtTokenResponse>> Login(LoginRequest request, CancellationToken ct)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = HttpContext.Request.Headers.UserAgent.ToString();

            var response = await _authService.Login(request, ipAddress, userAgent, ct);

            Response.Cookies.Append("refreshToken", response.RefreshToken!, RefreshCookieOptions(response.RefreshTokenExpiration));
            return response.ToJwtTokenResponse();
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
                return response.ToJwtTokenResponse();
            }
            catch (InvalidRefreshTokenException)
            {
                Response.Cookies.Delete("refreshToken", RefreshCookieOptions());
                throw;
            }
        }

        private static CookieOptions RefreshCookieOptions(DateTimeOffset? expires = null)
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
