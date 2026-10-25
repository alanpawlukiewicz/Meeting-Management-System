using MeetingManager.Core.Exceptions;
using MeetingManager.Core.ServiceContracts.UserServiceContracts;
using MeetingManager.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Core.Services.UserServices
{
    public class UserDeleterService : IUserDeleterService
    {
        private readonly IUserRepository _userRepository;

        public UserDeleterService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task DeleteUserAsync(Guid userId, CancellationToken ct = default)
        {
            var matchingUser = await _userRepository.FindUserWithRolesAsync(userId, ct)
                ?? throw new NotFoundException("User could not be found.");

            _userRepository.Remove(matchingUser);

            await _userRepository.SaveChangesAsync(ct);
        }
    }
}
