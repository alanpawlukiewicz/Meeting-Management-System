namespace MeetingManager.Core.DTO.UserDTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public List<string> RoleNames { get; set; } = new();

        public class UserResponse
        {
        }
    }
}
