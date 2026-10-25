using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetingManager.Domain.Entities
{
    public class RefreshToken
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public string TokenHash { get; set; } = null!;
        [Required]
        public DateTimeOffset ExpiresAt { get; set; }
        public string? UserAgent { get; set; }
        public string? IpAddress { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
    }
}
