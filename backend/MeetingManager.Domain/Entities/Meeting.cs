using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetingManager.Domain.Entities
{
    public class Meeting
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid CreatorId { get; set; }
        [Required]
        public DateOnly Date { get; set; }
        [Required]
        public TimeOnly StartTime { get; set; }
        [Required]
        public TimeOnly EndTime { get; set; }
        [Required]
        [MaxLength(200)]
        public string Place { get; set; } = null!;

        [ForeignKey(nameof(CreatorId))]
        public User? User { get; set; }
    }
}
