using System.ComponentModel.DataAnnotations;

namespace MeetingManager.Core.DTO.MeetingDTOs
{
    public class MeetingRequest
    {
        [Required]
        public DateOnly Date { get; set; }
        [Required]
        public TimeOnly StartTime { get; set; }
        [Required]
        public TimeOnly EndTime { get; set; }
        [Required]
        public string Place { get; set; } = null!;
        [Required]
        public string Title { get; set; } = null!;
        public List<Guid>? ParticipantIds { get; set; }
    }
}
