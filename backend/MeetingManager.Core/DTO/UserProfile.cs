namespace MeetingManager.Core.DTO
{
    public class UserProfile
    {
        public string UserName { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}
