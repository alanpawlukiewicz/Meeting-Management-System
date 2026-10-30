namespace MeetingManager.Core.DTO.MeetingDTOs
{
    public class MeetingResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string Place { get; set; } = string.Empty;
    }
}
