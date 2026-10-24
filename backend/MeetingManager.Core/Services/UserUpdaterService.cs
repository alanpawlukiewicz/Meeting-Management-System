using MeetingManager.Core.Constants;
using MeetingManager.Core.Exceptions;
using MeetingManager.Core.ServiceContracts;
using MeetingManager.Domain.Entities;
using MeetingManager.Domain.RepositoryContracts;

namespace MeetingManager.Core.Services
{
    public class UserUpdaterService : IUserUpdaterService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        public UserUpdaterService(IUserRepository userRepository, IRoleRepository roleRepository, IUserRoleRepository userRoleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
        }

        public async Task GiveUserOrganizerRoleAsync(Guid userId, CancellationToken ct = default)
        {
            var matchingUser = await _userRepository.FindUserWithRolesAsync(userId, ct);

            if (matchingUser == null)
                throw new NotFoundException("User could not be found.");

            var organizerRole = await _roleRepository.FindRoleByNameAsync(RoleName.Organizer, ct);

            if (organizerRole == null)
                throw new NotFoundException("Organizer role could not be found.");

            if (!matchingUser.UserRoles.Any(ur => ur.Role!.Name == RoleName.Organizer))
            {
                matchingUser.UserRoles.Add(new UserRole
                {
                    Id = Guid.NewGuid(),
                    RoleId = organizerRole.Id,
                    UserId = userId
                });

                await _userRepository.SaveChangesAsync(ct);
            }

            throw new ConflictException("User already has organizer role.");
        }

        public async Task RevokeUsersOrganizerRoleAsync(Guid userId, CancellationToken ct = default)
        {
            var matchingUser = await _userRepository.FindUserWithRolesAsync(userId, ct);

            if (matchingUser == null)
                throw new NotFoundException("User could not be found.");

            var matchingUserRole = matchingUser.UserRoles.FirstOrDefault(ur => ur.Role!.Name == RoleName.Organizer);

            if (matchingUserRole == null)
                throw new NotFoundException("User does not have organizer role.");

            _userRoleRepository.Remove(matchingUserRole);

            await _userRoleRepository.SaveChangesAsync(ct);
        }
    }
}
