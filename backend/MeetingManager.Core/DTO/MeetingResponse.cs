using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Core.DTO
{
    public class MeetingResponse
    {
        public Guid Id { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string Place { get; set; } = string.Empty;
    }
}
