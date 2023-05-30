using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backend.Models
{
    public class Meeting
    {
        [JsonIgnore]
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Time { get; set; }
        [Required]
        public int Duration { get; set; } // In minutes
        public string? Location { get; set; }
        [Required]
        public MeetingType Type { get; set; }
        public string? Description { get; set; }
        public string? MeetingNotes { get; set; }
        [Required]
        public string ClientName { get; set; }
    }
}
