using System.ComponentModel.DataAnnotations;

namespace MeetingManager.Domain.Entities
{
    public class Role
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
