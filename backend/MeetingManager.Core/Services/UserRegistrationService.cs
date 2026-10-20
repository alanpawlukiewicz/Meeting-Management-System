using MeetingManager.Core.DTO;
using MeetingManager.Core.Exceptions;
using MeetingManager.Core.Mappers;
using MeetingManager.Core.ServiceContracts;
using MeetingManager.Domain.Entities;
using MeetingManager.Domain.RepositoryContracts;

namespace MeetingManager.Core.Services
{
    public class UserRegistrationService : IUserRegistrationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UserRegistrationService(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserDto> RegisterUserAsync(UserRegisterRequest request, CancellationToken ct = default)
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

            return newUser.ToUserDto();
        }
    }
}
