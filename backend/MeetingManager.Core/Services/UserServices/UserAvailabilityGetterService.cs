using MeetingManager.Core.DTO.UserDTOs;
using MeetingManager.Core.Mappers;
using MeetingManager.Core.ServiceContracts.UserServiceContracts;
using MeetingManager.Domain.RepositoryContracts;

namespace MeetingManager.Core.Services.UserServices
{
    public class UserAvailabilityGetterService : IUserAvailabilityGetterService
    {
        private readonly IUserAvailabilityRepository _userAvailabilityRepository;
        private readonly IUserTimeOffRepository _userTimeOffRepository;

        public UserAvailabilityGetterService(IUserAvailabilityRepository userAvailabilityRepository, IUserTimeOffRepository userTimeOffRepository)
        {
            _userAvailabilityRepository = userAvailabilityRepository;
            _userTimeOffRepository = userTimeOffRepository;
        }

        public async Task<UserAvailabilityDetails> GetUserAvailabilitiesAsync(Guid userId, CancellationToken ct = default)
        {
            var matchingAvailabilities = await _userAvailabilityRepository.GetUserAvailabilitiesAsync(userId, ct);
            var matchingTimeOffs = await _userTimeOffRepository.GetUserTimeOffsAsync(userId, ct);
            return new UserAvailabilityDetails
            {
                Availabilities = matchingAvailabilities.Select(ua => ua.ToUserAvailabilityDto()).ToList(),
                TimeOffs = matchingTimeOffs.Select(uto => uto.ToUserTimeOffResponse()).ToList()
            };
        }
    }
}
