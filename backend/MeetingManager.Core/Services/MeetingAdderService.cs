using MeetingManager.Core.DTO;
using MeetingManager.Core.Mappers;
using MeetingManager.Core.ServiceContracts;
using MeetingManager.Domain.Entities;
using MeetingManager.Domain.RepositoryContracts;
using System.ComponentModel.DataAnnotations;

namespace MeetingManager.Core.Services
{
    public class MeetingAdderService : IMeetingAdderService
    {
        private readonly IMeetingRepository _meetingRepository;
        private readonly IUserRepository _userRepository;

        public MeetingAdderService(IMeetingRepository meetingRepository, IUserRepository userRepository)
        {
            _meetingRepository = meetingRepository;
            _userRepository = userRepository;
        }

        public async Task<MeetingResponse> AddMeetingAsync(Guid creatorId, MeetingRequest request, CancellationToken ct = default)
        {
            if (request.StartTime > request.EndTime)
                throw new ValidationException("StartTime must be earlier than EndTime.");

            if (request.Date < DateOnly.FromDateTime(DateTime.UtcNow))
                throw new ValidationException("Given date is expired.");

            Meeting newMeeting = new Meeting
            {
                Id = Guid.NewGuid(),
                CreatorId = creatorId,
                Date = request.Date,
                EndTime = request.EndTime,
                Place = request.Place,
                StartTime = request.StartTime
            };

            if (request.ParticipantIds != null)
            {
                if (!request.ParticipantIds.Contains(creatorId))
                    newMeeting.MeetingParticipants.Add(new MeetingParticipant
                    {
                        Id = Guid.NewGuid(),
                        RespondedAt = DateTimeOffset.UtcNow,
                        Status = Domain.Enums.AttendanceStatus.Accepted,
                        UserId = creatorId
                    });

                newMeeting.MeetingParticipants = request.ParticipantIds.Distinct().Select(DefaultMeetingParticipant).ToList();
            }
            else
            {
                List<Guid> allUserIds = await _userRepository.GetAllUserIdsAsync(ct);
                newMeeting.MeetingParticipants = allUserIds.Select(DefaultMeetingParticipant).ToList();
            }

            _meetingRepository.Add(newMeeting);

            await _meetingRepository.SaveChangesAsync(ct);

            return newMeeting.ToMeetingResponse();
        }

        private static MeetingParticipant DefaultMeetingParticipant(Guid userId)
        {
            return new MeetingParticipant
            {
                Id = Guid.NewGuid(),
                RespondedAt = null,
                Status = Domain.Enums.AttendanceStatus.Pending,
                UserId = userId
            };
        }
    }
}
