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
                newMeeting.MeetingParticipants = request.ParticipantIds.Distinct().Select(DefaultMeetingParticipant).ToList();

                if (!request.ParticipantIds.Contains(creatorId))
                    newMeeting.MeetingParticipants.Add(new MeetingParticipant
                    {
                        Id = Guid.NewGuid(),
                        RespondedAt = DateTimeOffset.UtcNow,
                        Status = Domain.Enums.AttendanceStatus.Accepted,
                        UserId = creatorId
                    });
            }
            else
            {
                List<Guid> allUserIds = await _userRepository.GetAllUserIdsAsync(ct);
                var parsedParticipants = allUserIds.Select(DefaultMeetingParticipant).ToList();
                var creator = parsedParticipants.Single(p => p.UserId == creatorId);
                creator.RespondedAt = DateTimeOffset.UtcNow;
                creator.Status = Domain.Enums.AttendanceStatus.Accepted;
                newMeeting.MeetingParticipants = parsedParticipants;
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
