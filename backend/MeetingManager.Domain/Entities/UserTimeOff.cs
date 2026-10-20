using System.ComponentModel.DataAnnotations;

namespace MeetingManager.Domain.Entities
{
    public class UserTimeOff
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public DateOnly Date { get; set; }
        [Required]
        public TimeOnly StartTime { get; set; }
        [Required]
        public TimeOnly EndTime { get; set; }
        [Required]
        public bool IsAvailable { get; set; } = true;
    }
}
