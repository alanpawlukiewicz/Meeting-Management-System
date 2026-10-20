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
        [MinLength(8, ErrorMessage = "Password must have at least 8 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "Password must contain a digit, small and big letter.")]
        public string Password { get; set; } = null!;
    }
}
