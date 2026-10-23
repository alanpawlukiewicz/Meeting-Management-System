using MeetingManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MeetingManager.Core.DTO
{
    public class StatusDto
    {
        [Required]
        public AttendanceStatus Status { get; set; }
    }
}
