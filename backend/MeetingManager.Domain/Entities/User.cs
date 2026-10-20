using System.ComponentModel.DataAnnotations;

namespace MeetingManager.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        [MaxLength(256)]
        public string UserName { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PasswordHash { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public int AccessFailedCount { get; set; } = 0;

        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
