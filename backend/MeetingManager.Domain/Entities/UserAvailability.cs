using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MeetingManager.Domain.Entities
{
    public class UserAvailability
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public DayOfWeek DayOfWeek { get; set; }
        [Required]
        public TimeOnly StartTime { get; set; }
        [Required]
        public TimeOnly EndTime { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
    }
}
