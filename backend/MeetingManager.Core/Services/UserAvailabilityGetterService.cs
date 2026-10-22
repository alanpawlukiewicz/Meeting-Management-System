using MeetingManager.Core.DTO;
using MeetingManager.Core.Mappers;
using MeetingManager.Core.ServiceContracts;
using MeetingManager.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Core.Services
{
    public class UserAvailabilityGetterService : IUserAvailabilityGetterService
    {
        private readonly IUserAvailabilityRepository _userAvailabilityRepository;

        public UserAvailabilityGetterService(IUserAvailabilityRepository userAvailabilityRepository)
        {
            _userAvailabilityRepository = userAvailabilityRepository;
        }

        public async Task<List<UserAvailabilityDto>> GetUserAvailabilitiesAsync(Guid userId, CancellationToken ct = default)
        {
            return (await _userAvailabilityRepository.GetUserAvailabilitiesAsync(userId, ct)).Select(ua => ua.ToUserAvailabilityDto()).ToList();
        }
    }
}
