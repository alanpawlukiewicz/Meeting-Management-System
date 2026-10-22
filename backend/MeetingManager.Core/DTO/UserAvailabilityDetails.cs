using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Core.DTO
{
    
    public class UserAvailabilityDetails
    {
        public List<UserAvailabilityDto> Availabilities { get; set; } = new();
        public List<UserTimeOffResponse> TimeOffs { get; set; } = new();
    }
}
