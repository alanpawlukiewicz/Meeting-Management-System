using System.ComponentModel.DataAnnotations;

namespace MeetingManager.Core.DTO
{
    public class UserRegisterRequest
    {
        [Required]
        public string UserName { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [Required]
        public string Password { get; set; } = null!;
    }
}
