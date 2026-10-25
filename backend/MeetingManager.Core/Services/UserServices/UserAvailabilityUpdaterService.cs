using MeetingManager.Core.DTO;
using MeetingManager.Core.ServiceContracts.UserServiceContracts;
using MeetingManager.Domain.Entities;
using MeetingManager.Domain.RepositoryContracts;

namespace MeetingManager.Core.Services.UserServices
{
    public class UserAvailabilityUpdaterService : IUserAvailabilityUpdaterService
    {
        private readonly IUserAvailabilityRepository _userAvailabilityRepository;

        public UserAvailabilityUpdaterService(IUserAvailabilityRepository userAvailabilityRepository)
        {
            _userAvailabilityRepository = userAvailabilityRepository;
        }

        public async Task<List<UserAvailabilityDto>> UpdateUserAvailabilityListAsync(Guid userId, List<UserAvailabilityDto> requestList, CancellationToken ct = default)
        {
            await _userAvailabilityRepository.SetCurrentUserAvailabilitiesForDeleteAsync(userId, ct);

            foreach (var availability in requestList)
            {
                _userAvailabilityRepository.Add(new UserAvailability
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    DayOfWeek = availability.DayOfWeek,
                    EndTime = availability.EndTime,
                    StartTime = availability.StartTime
                });
            }

            await _userAvailabilityRepository.SaveChangesAsync(ct);

            return requestList;
        }
    }
}
