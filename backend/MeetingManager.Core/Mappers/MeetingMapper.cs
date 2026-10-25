using MeetingManager.Core.DTO;
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
                CreatorName = meeting.User?.UserName ?? string.Empty,
                Date = meeting.Date,
                EndTime = meeting.EndTime,
                StartTime = meeting.StartTime,
                Place = meeting.Place,
                Status = meeting.MeetingParticipants.FirstOrDefault(mp => mp.UserId == userId)!.Status.ToString()
            };
        }

        public static MeetingResponse ToMeetingResponse(this Meeting meeting)
        {
            return new MeetingResponse
            {
                Id = meeting.Id,
                Date = meeting.Date,
                EndTime = meeting.EndTime,
                StartTime = meeting.StartTime,
                Place = meeting.Place
            };
        }
    }
}
