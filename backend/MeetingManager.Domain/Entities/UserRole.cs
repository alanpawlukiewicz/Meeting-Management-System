using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetingManager.Domain.Entities
{
    public class UserRole
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid RoleId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
        [ForeignKey(nameof(RoleId))]
        public Role? Role { get; set; }
    }
}
