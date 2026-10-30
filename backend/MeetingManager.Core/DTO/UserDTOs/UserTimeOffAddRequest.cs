using System.ComponentModel.DataAnnotations;

namespace MeetingManager.Core.DTO.UserDTOs
{
    public class UserTimeOffAddRequest
    {
        [Required]
        public DateOnly Date { get; set; }
        [Required]
        public TimeOnly StartTime { get; set; }
        [Required]
        public TimeOnly EndTime { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
