using MeetingManager.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetingManager.Domain.Entities
{
    public class MeetingParticipant
    {
        public Guid Id { get; set; }
        [Required]
        public Guid MeetingId { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public bool WillAttend { get; set; }
        public AttendanceStatus Status { get; private set; } = AttendanceStatus.Pending;
        public DateTimeOffset? RespondedAt { get; private set; }

        [ForeignKey(nameof(MeetingId))]
        public Meeting? Meeting { get; set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
    }
}
