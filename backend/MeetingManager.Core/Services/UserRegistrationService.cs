using MeetingManager.Core.DTO;
using MeetingManager.Core.Exceptions;
using MeetingManager.Core.Mappers;
using MeetingManager.Core.ServiceContracts;
using MeetingManager.Domain.Entities;
using MeetingManager.Domain.RepositoryContracts;
using System.Net;

namespace MeetingManager.Core.Services
{
    public class UserRegistrationService : IUserRegistrationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenAdderService _refreshTokenAdderService;

        public UserRegistrationService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtService jwtService, IRefreshTokenAdderService refreshTokenAdderService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
            _refreshTokenAdderService = refreshTokenAdderService;
        }

        public async Task<AuthResponse> RegisterUserAsync(UserRegisterRequest request, string? ipAddress, string? userAgent, CancellationToken ct = default)
        {
            var matchingUser = await _userRepository.FindUserByLoginAsync(request.UserName, ct);

            if (matchingUser != null)
            {
                throw new ConflictException("User with given login already exists.");
            }


            User newUser = new User
            {
                Id = Guid.NewGuid(),
                UserName = request.UserName,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PasswordHash = _passwordHasher.Hash(request.Password)
            };

            _userRepository.Add(newUser);

            await _userRepository.SaveChangesAsync(ct);

            var authResponse = await _jwtService.GenerateJwtToken(newUser, ct);

            var refreshTokenAddRequest = new RefreshTokenAddRequest
            {
                UserId = newUser.Id,
                UserAgent = userAgent,
                IpAddress = ipAddress,
                Token = authResponse.RefreshToken,
                ExpiresAt = authResponse.RefreshTokenExpiration
            };

            await _refreshTokenAdderService.AddTokenAsync(refreshTokenAddRequest, ct);

            return authResponse;
        }
    }
}
