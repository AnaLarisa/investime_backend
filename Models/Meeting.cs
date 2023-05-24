namespace backend.Models
{
    public class Meeting
    {
        public Guid Id { get; } = Guid.NewGuid();
        public DateTime Date { get; set; }
        public string? Time { get; set; }
        public int Duration { get; set; } // In minutes
        public string? Location { get; set; }
        public MeetingType Type { get; set; }
        public string? Description { get; set; }
        public string? MeetingNotes { get; set; }
        public string? ClientName { get; set; }
    }
}
