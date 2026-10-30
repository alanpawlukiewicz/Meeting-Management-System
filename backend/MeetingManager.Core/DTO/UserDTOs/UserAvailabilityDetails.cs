namespace MeetingManager.Core.DTO.UserDTOs
{

    public class UserAvailabilityDetails
    {
        public List<UserAvailabilityDto> Availabilities { get; set; } = new();
        public List<UserTimeOffResponse> TimeOffs { get; set; } = new();
    }
}
