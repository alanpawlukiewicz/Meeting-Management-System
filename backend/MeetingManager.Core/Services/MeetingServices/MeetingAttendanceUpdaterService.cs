using MeetingManager.Core.DTO;
using MeetingManager.Core.Exceptions;
using MeetingManager.Core.ServiceContracts.MeetingServiceContracts;
using MeetingManager.Domain.Enums;
using MeetingManager.Domain.RepositoryContracts;

namespace MeetingManager.Core.Services.MeetingServices
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

            matchingParticipant.Status = Enum.TryParse<AttendanceStatus>(request.Status, out var status)
                ? status
                : throw new ConflictException("Unsupported attendance status.");

            await _meetingParticipantRepository.SaveChangesAsync(ct);
        }
    }
}
