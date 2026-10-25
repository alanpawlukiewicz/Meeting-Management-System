using MeetingManager.Core.DTO;
using MeetingManager.Core.Exceptions;
using MeetingManager.Core.Mappers;
using MeetingManager.Core.ServiceContracts.MeetingServiceContracts;
using MeetingManager.Domain.Entities;
using MeetingManager.Domain.Enums;
using MeetingManager.Domain.RepositoryContracts;

namespace MeetingManager.Core.Services.MeetingServices
{
    public class MeetingUpdaterService : IMeetingUpdaterService
    {
        private readonly IMeetingRepository _meetingRepository;
        private readonly IMeetingParticipantRepository _meetingParticipantRepository;
        private readonly IUserRepository _userRepository;

        public MeetingUpdaterService(IMeetingRepository meetingRepository, IMeetingParticipantRepository meetingParticipantRepository, IUserRepository userRepository)
        {
            _meetingRepository = meetingRepository;
            _meetingParticipantRepository = meetingParticipantRepository;
            _userRepository = userRepository;
        }

        public async Task<MeetingResponse> UpdateMeetingAsync(
                Guid meetingId, Guid creatorId, MeetingRequest request, CancellationToken ct = default)
        {
            if (request.EndTime < request.StartTime)
                throw new ValidationException("End time must be later than start time.");

            if (request.Date < DateOnly.FromDateTime(DateTime.UtcNow))
                throw new ValidationException("Given date is expired.");

            var meeting = await _meetingRepository.FindMeetingForUpdateAsync(meetingId, ct)
                ?? throw new NotFoundException("Could not find meeting with given id.");

            if (meeting.CreatorId != creatorId)
                throw new UnauthorizedAccessException("Only original creator can update this meeting.");

            var scheduleChanged =
                meeting.Date != request.Date ||
                meeting.StartTime != request.StartTime ||
                meeting.EndTime != request.EndTime ||
                meeting.Place != request.Place;

            meeting.Date = request.Date;
            meeting.StartTime = request.StartTime;
            meeting.EndTime = request.EndTime;
            meeting.Place = request.Place;

            if (request.ParticipantIds != null)
            {
                var requestedIds = request.ParticipantIds.ToHashSet();

                var toRemove = meeting.MeetingParticipants
                    .Where(mp => !requestedIds.Contains(mp.UserId))
                    .ToList();

                foreach (var mp in toRemove)
                {
                    _meetingParticipantRepository.Remove(mp);
                }

                var existingIds = meeting.MeetingParticipants.Select(mp => mp.UserId).ToHashSet();
                foreach (var id in requestedIds.Except(existingIds))
                {
                    meeting.MeetingParticipants.Add(DefaultMeetingParticipant(id));
                }


                if (!request.ParticipantIds.Contains(creatorId))
                    meeting.MeetingParticipants.Add(new MeetingParticipant
                    {
                        Id = Guid.NewGuid(),
                        RespondedAt = DateTimeOffset.UtcNow,
                        Status = AttendanceStatus.Accepted,
                        UserId = creatorId
                    });
            }
            else
            {
                List<Guid> allUserIds = await _userRepository.GetAllUserIdsAsync(ct);
                meeting.MeetingParticipants = allUserIds.Select(DefaultMeetingParticipant).ToList();
            }

            if (scheduleChanged)
            {
                foreach (var mp in meeting.MeetingParticipants.Where(mp => mp.Status != AttendanceStatus.Pending))
                {
                    mp.Status = AttendanceStatus.Pending;
                    mp.RespondedAt = null;
                }
            }

            await _meetingRepository.SaveChangesAsync(ct);
            return meeting.ToMeetingResponse();
        }

        private static MeetingParticipant DefaultMeetingParticipant(Guid userId)
        {
            return new MeetingParticipant
            {
                Id = Guid.NewGuid(),
                RespondedAt = null,
                Status = AttendanceStatus.Pending,
                UserId = userId
            };
        }
    }
}
