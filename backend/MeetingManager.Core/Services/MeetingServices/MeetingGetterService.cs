using MeetingManager.Core.DTO.MeetingDtos;
using MeetingManager.Core.Mappers;
using MeetingManager.Core.ServiceContracts.MeetingServiceContracts;
using MeetingManager.Domain.RepositoryContracts;

namespace MeetingManager.Core.Services.MeetingServices
{
    public class MeetingGetterService : IMeetingGetterService
    {
        private readonly IMeetingRepository _meetingRepository;

        public MeetingGetterService(IMeetingRepository meetingRepository)
        {
            _meetingRepository = meetingRepository;
        }

        public async Task<List<MeetingDto>> GetCurrentUserMeetingsAsync(Guid userId, CancellationToken ct = default)
        {
            var meetings = await _meetingRepository.GetCurrentParticipantMeetingsAsync(userId, ct);
            return meetings.Select(m => m.ToMeetingDto(userId)).ToList();
        }
    }
}
