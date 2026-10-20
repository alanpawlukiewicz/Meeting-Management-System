using MeetingManager.Core.DTO;
using MeetingManager.Core.Exceptions;
using MeetingManager.Core.ServiceContracts;
using MeetingManager.Domain.Entities;
using MeetingManager.Domain.RepositoryContracts;

namespace MeetingManager.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenAdderService _refreshTokenAdderService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IRefreshTokenRotationService _refreshTokenRotationService;
        private readonly ITokenHasher _tokenHasher;
        private const int _maxFailedAccessAttempts = 5;
        private const int _lockoutMinutes = 5;

        public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtService jwtService, IRefreshTokenAdderService refreshTokenAdderService, ITokenHasher tokenHasher, IRefreshTokenRepository refreshTokenRepository, IRefreshTokenRotationService refreshTokenRotationService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
            _refreshTokenAdderService = refreshTokenAdderService;
            _tokenHasher = tokenHasher;
            _refreshTokenRepository = refreshTokenRepository;
            _refreshTokenRotationService = refreshTokenRotationService;
        }

        public async Task<AuthResponse> Login(LoginRequest request, string? ipAddress, string? userAgent, CancellationToken ct = default)
        {
            var matchingUser = await _userRepository.FindUserByLoginAsync(request.UserName, ct);

            if (matchingUser == null)
            {
                _passwordHasher.Hash(request.Password);
                throw new InvalidCredentialsException("Invalid login or password.");
            }

            if (matchingUser.LockoutEnd != null && matchingUser.LockoutEnd.Value > DateTimeOffset.UtcNow)
            {
                throw new TooManyRequestsException("Account is temporarily locked due to too many invalid login requests. Try again later.");
            }

            var successfulLogin = await VerifyUserAsync(matchingUser, request.Password, ct);

            if (!successfulLogin)
            {
                throw new InvalidCredentialsException("Invalid login or password.");
            }

            var authResponse = await _jwtService.GenerateJwtToken(matchingUser, ct);

            var refreshTokenAddRequest = new RefreshTokenAddRequest
            {
                UserId = matchingUser.Id,
                UserAgent = userAgent,
                IpAddress = ipAddress,
                Token = authResponse.RefreshToken,
                ExpiresAt = authResponse.RefreshTokenExpiration
            };

            await _refreshTokenAdderService.AddTokenAsync(refreshTokenAddRequest, ct);

            return authResponse;
        }

        private async Task<bool> VerifyUserAsync(User user, string password, CancellationToken ct)
        {
            var successfulLogin = _passwordHasher.Verify(password, user.PasswordHash);
            if (successfulLogin)
            {
                if (user.AccessFailedCount > 0 || user.LockoutEnd != null)
                {
                    user.AccessFailedCount = 0;
                    user.LockoutEnd = null;
                    await _userRepository.SaveChangesAsync(ct);
                }

                return true;
            }

            user.AccessFailedCount += 1;
            if (user.AccessFailedCount >= _maxFailedAccessAttempts)
            {
                user.LockoutEnd = DateTimeOffset.UtcNow.AddMinutes(_lockoutMinutes);
                user.AccessFailedCount = 0;
            }
            await _userRepository.SaveChangesAsync(ct);

            return false;
        }

        public async Task<AuthResponse> RefreshTokensAsync(string? refreshToken, string? ipAddress, string? userAgent, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                throw new InvalidRefreshTokenException("Refresh token is missing from cookies.");
            }

            var hashedToken = _tokenHasher.Hash(refreshToken);

            var matchingToken = await _refreshTokenRepository.FindTokenByHashAsync(hashedToken, ct);

            if (matchingToken == null || matchingToken.ExpiresAt <= DateTimeOffset.UtcNow)
            {
                throw new InvalidRefreshTokenException("Invalid refresh token.");
            }

            var matchingUser = await _userRepository.FindUserAsync(matchingToken.UserId, ct);

            if (matchingUser == null || (matchingUser.LockoutEnd != null && matchingUser.LockoutEnd > DateTimeOffset.UtcNow))
            {
                throw new InvalidRefreshTokenException("Invalid refresh token.");
            }

            var authResponse = await _jwtService.GenerateJwtToken(matchingUser, ct);

            _refreshTokenRepository.Remove(matchingToken);
            var refreshTokenAddRequest = new RefreshTokenAddRequest
            {
                UserId = matchingUser.Id,
                UserAgent = userAgent,
                IpAddress = ipAddress,
                Token = authResponse.RefreshToken,
                ExpiresAt = authResponse.RefreshTokenExpiration
            };

            await _refreshTokenRotationService.RotateAsync(matchingToken, refreshTokenAddRequest, ct);

            return authResponse;
        }
    }
}
