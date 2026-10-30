using MeetingManager.Core.DTO.MeetingDtos;
using MeetingManager.Core.DTO.MeetingDTOs;
using MeetingManager.Domain.Entities;

namespace MeetingManager.Core.Mappers
{
    public static class MeetingMapper
    {
        public static MeetingDto ToMeetingDto(this Meeting meeting, Guid userId)
        {
            return new MeetingDto
            {
                Id = meeting.Id,
                CreatorId = meeting.CreatorId,
                Title = meeting.Title,
                CreatorName = meeting.User?.UserName ?? string.Empty,
                Date = meeting.Date,
                EndTime = meeting.EndTime,
                StartTime = meeting.StartTime,
                Place = meeting.Place,
                Status = meeting.MeetingParticipants.FirstOrDefault(mp => mp.UserId == userId)!.Status.ToString(),
                Participants = meeting.MeetingParticipants.Select(mp => mp.User!.ToUserResponse()).ToList()
            };
        }

        public static MeetingResponse ToMeetingResponse(this Meeting meeting)
        {
            return new MeetingResponse
            {
                Id = meeting.Id,
                Date = meeting.Date,
                Title = meeting.Title,
                EndTime = meeting.EndTime,
                StartTime = meeting.StartTime,
                Place = meeting.Place
            };
        }
    }
}
