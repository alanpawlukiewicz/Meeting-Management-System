namespace MeetingManager.Core.DTO
{
    public class MeetingDto
    {
        public Guid Id { get; set; }
        public string CreatorName { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string Place { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
