using System.ComponentModel.DataAnnotations;

namespace MeetingManager.Domain.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [MaxLength(200)]
        [Required]
        public string UserName { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [Required]
        public string PasswordHash { get; set; } = null!;
        public DateTimeOffset? LockoutEnd { get; set; }
        public int AccessFailedCount { get; set; } = 0;

        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
