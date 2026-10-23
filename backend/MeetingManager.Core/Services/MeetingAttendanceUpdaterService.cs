using MeetingManager.Core.DTO;
using MeetingManager.Core.Exceptions;
using MeetingManager.Core.ServiceContracts;
using MeetingManager.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Core.Services
{
    public class MeetingAttendanceUpdaterService : IMeetingAttendanceUpdaterService
    {
        private readonly IMeetingParticipantRepository _meetingParticipantRepository;

        public MeetingAttendanceUpdaterService(IMeetingParticipantRepository meetingParticipantRepository)
        {
            _meetingParticipantRepository = meetingParticipantRepository;
        }

        public async Task ChangeAttendanceAsync(Guid meetingId, Guid userId, StatusDto request, CancellationToken ct = default)
        {
            var matchingParticipant = await _meetingParticipantRepository.FindMeetingParticipantByMeetingAndUserIdsAsync(meetingId, userId, ct);

            if (matchingParticipant == null)
                throw new NotFoundException("Could not find meeting participant record.");

            matchingParticipant.Status = request.Status;

            await _meetingParticipantRepository.SaveChangesAsync(ct);
        }
    }
}
