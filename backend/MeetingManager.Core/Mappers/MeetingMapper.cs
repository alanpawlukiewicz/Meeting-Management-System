using MeetingManager.Core.DTO;
using MeetingManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Core.Mappers
{
    public static class MeetingMapper
    {
        public static MeetingDto ToMeetingDto(this Meeting meeting)
        {
            return new MeetingDto
            {
                Id = meeting.Id,
                CreatorName = meeting.User?.UserName ?? string.Empty,
                Date = meeting.Date,
                EndTime = meeting.EndTime,
                StartTime = meeting.StartTime,
                Place = meeting.Place
            };
        }
    }
}
