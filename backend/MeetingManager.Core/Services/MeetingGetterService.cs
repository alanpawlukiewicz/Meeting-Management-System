using MeetingManager.Core.DTO;
using MeetingManager.Core.Mappers;
using MeetingManager.Core.ServiceContracts;
using MeetingManager.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Core.Services
{
    public class MeetingGetterService : IMeetingGetterService
    {
        private readonly IMeetingRepository _meetingRepository;

        public MeetingGetterService(IMeetingRepository meetingRepository)
        {
            _meetingRepository = meetingRepository;
        }

        public async Task<List<MeetingDto>> GetUserMeetingsAsync(Guid userId, CancellationToken ct = default)
        {
            var meetings = await _meetingRepository.GetParticipantMeetingsAsync(userId, ct);
            return meetings.Select(m => m.ToMeetingDto()).ToList();
        }
    }
}
