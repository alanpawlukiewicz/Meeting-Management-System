namespace MeetingManager.Core.DTO
{
    public class UserStatusCheckRequest
    {
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public List<Guid>? UserIds { get; set; }
    }
}
