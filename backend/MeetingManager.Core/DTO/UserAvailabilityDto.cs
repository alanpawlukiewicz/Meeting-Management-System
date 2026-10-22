using System.ComponentModel.DataAnnotations;

namespace MeetingManager.Core.DTO
{
    public class UserAvailabilityDto
    {
        [Required]
        public DayOfWeek DayOfWeek { get; set; }
        [Required]
        public TimeOnly StartTime { get; set; }
        [Required]
        public TimeOnly EndTime { get; set; }
    }
}
