using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MeetingManager.Core.DTO
{
    public class PasswordUpdateRequest
    {
        [Required]
        public string OldPassword { get; set; } = null!;
        [Required]
        [MinLength(8, ErrorMessage = "Password must have at least 8 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "Password must contain a digit, small and big letter.")]
        public string NewPassword { get; set; } = null!;
    }
}
