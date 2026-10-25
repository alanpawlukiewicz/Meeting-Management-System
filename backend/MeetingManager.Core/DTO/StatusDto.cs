using System.ComponentModel.DataAnnotations;

namespace MeetingManager.Core.DTO
{
    public class StatusDto
    {
        [Required]
        public string Status { get; set; } = null!;
    }
}
