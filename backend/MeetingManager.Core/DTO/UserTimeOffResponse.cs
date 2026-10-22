using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Core.DTO
{
    public class UserTimeOffResponse
    {
        public Guid Id { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
