using MeetingManager.Core.DTO;
using MeetingManager.Core.Exceptions;
using MeetingManager.Core.Mappers;
using MeetingManager.Core.ServiceContracts;
using MeetingManager.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Core.Services
{
    public class UserGetterService : IUserGetterService
    {
        private readonly IUserRepository _userRepository;

        public UserGetterService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserProfile> GetUserProfileAsync(Guid userId, CancellationToken ct = default)
        {
            var matchingUser = await _userRepository.FindUserWithRolesAsync(userId, ct);

            if (matchingUser == null)
            {
                throw new NotFoundException("User could not be found.");
            }

            return matchingUser.ToUserProfile();
        }
    }
}
