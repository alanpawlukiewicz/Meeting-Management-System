using MeetingManager.Core.Constants;
using MeetingManager.Core.DTO.UserDTOs;
using MeetingManager.Core.Exceptions;
using MeetingManager.Core.Mappers;
using MeetingManager.Core.ServiceContracts.UserServiceContracts;
using MeetingManager.Domain.Entities;
using MeetingManager.Domain.RepositoryContracts;

namespace MeetingManager.Core.Services.UserServices
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
            var user = await _userRepository.FindUserWithRolesAsync(userId, ct)
                ?? throw new NotFoundException("User could not be found.");

            var organizerRole = await _roleRepository.FindRoleByNameAsync(RoleName.Organizer, ct)
                ?? throw new NotFoundException("Organizer role could not be found.");

            if (user.UserRoles.Any(ur => ur.RoleId == organizerRole.Id))
                throw new ConflictException("User already has organizer role.");

            _userRoleRepository.Add(new UserRole
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                RoleId = organizerRole.Id
            });

            await _userRoleRepository.SaveChangesAsync(ct);
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

        public async Task<UserResponse> UpdateUserProfile(Guid userId, UserUpdateRequest request, CancellationToken ct = default)
        {
            var matchingUser = await _userRepository.FindUserAsync(userId, ct);

            if (matchingUser == null)
                throw new NotFoundException("User could not be found.");

            matchingUser.UserName = request.UserName ?? matchingUser.UserName;
            matchingUser.FirstName = request.FirstName ?? matchingUser.FirstName;
            matchingUser.LastName = request.LastName ?? matchingUser.LastName;

            await _userRepository.SaveChangesAsync(ct);

            return matchingUser.ToUserResponse();
        }
    }
}
