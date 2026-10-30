namespace MeetingManager.Core.DTO.UserDTOs
{
    public class UserStatusCheckResponse
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
    }
}
