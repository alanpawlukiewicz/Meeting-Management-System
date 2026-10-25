using MeetingManager.Core.DTO;
using MeetingManager.Core.Exceptions;
using MeetingManager.Core.Mappers;
using MeetingManager.Core.ServiceContracts.UserServiceContracts;
using MeetingManager.Domain.Entities;
using MeetingManager.Domain.RepositoryContracts;

namespace MeetingManager.Core.Services.UserServices
{
    public class UserGetterService : IUserGetterService
    {
        private readonly IUserRepository _userRepository;

        public UserGetterService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserStatusCheckResponse>> CheckUserAvailabilitiesAsync(UserStatusCheckRequest request, CancellationToken ct = default)
        {
            if (request.EndTime < request.StartTime)
                throw new ValidationException("End time must be later than start time.");

            if (request.Date < DateOnly.FromDateTime(DateTime.UtcNow))
                throw new ValidationException("Given date is expired.");

            List<User> matchingUsers = await _userRepository.GetUsersWithAvailabilitiesAsync(request.UserIds?.Distinct().ToList(), request.Date, ct);

            List<UserStatusCheckResponse> response = new(matchingUsers.Count);

            var start = request.StartTime;
            var end = request.EndTime;

            foreach (var user in matchingUsers)
            {
                var freeWindows = user.UserAvailabilities
                    .Select(ua => (ua.StartTime, ua.EndTime))
                    .Concat(user.UserTimeOffs
                        .Where(t => t.IsAvailable)
                        .Select(t => (t.StartTime, t.EndTime)));

                bool covered = IsFullyCovered(freeWindows, start, end);

                bool blocked = user.UserTimeOffs
                    .Any(t => !t.IsAvailable && t.StartTime < end && t.EndTime > start);

                response.Add(new UserStatusCheckResponse
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    IsAvailable = covered && !blocked
                });
            }

            return response;
        }

        private static bool IsFullyCovered(
                IEnumerable<(TimeOnly Start, TimeOnly End)> windows, TimeOnly start, TimeOnly end)
        {
            var covered = start;

            foreach (var w in windows.Where(w => w.End > start && w.Start < end).OrderBy(w => w.Start))
            {
                if (w.Start > covered)
                    return false;

                if (w.End > covered)
                    covered = w.End;

                if (covered >= end)
                    return true;
            }

            return covered >= end;
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

        public async Task<List<LookupDto>> GetUsersForLookupAsync(string query, int limit = 10, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<LookupDto>();

            limit = Math.Clamp(limit, 1, 100);

            var users = await _userRepository.GetUsersWithLimitAsync(query.Trim().ToLower(), limit, ct);

            return users.Select(u => new LookupDto
            {
                Id = u.Id,
                Name = string.IsNullOrWhiteSpace(u.FirstName) || string.IsNullOrWhiteSpace(u.LastName)
                    ? u.UserName
                    : $"{u.UserName} ({u.FirstName} {u.LastName})"
            }).ToList();
        }

        public async Task<List<UserDto>> GetAllUsersWithRolesAsync(CancellationToken ct = default)
        {
            var result = await _userRepository.GetAllUsersWithRolesAsync(ct);

            return result.Select(u => u.ToUserDto()).ToList();
        }

        public async Task<bool> CheckUserNameAvailbility(string username, CancellationToken ct = default)
        {
            return await _userRepository.FindUserByLoginAsync(username, ct) == null;
        }
    }
}
