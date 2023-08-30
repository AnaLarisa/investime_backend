using System.ComponentModel.DataAnnotations;
using InvesTime.BackEnd.Validators;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InvesTime.BackEnd.Models;

public class Meeting
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public string Title { get; set; } = "Meeting";

    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime Date { get; set; } = DateTime.UtcNow;

    [Required]
    [DataType(DataType.Time)]
    [RegularExpression(@"^(?:[01]\d|2[0-3]):[0-5]\d$", ErrorMessage = "Invalid time format. Please use HH:mm format.")]
    public string Time { get; set; } = string.Empty;

    [Required]
    [Range(1, 480, ErrorMessage = "Value must be greater than zero.")]
    public int Duration { get; set; } = 0; // In minutes

    public string? Location { get; set; } = "Google Meets link";

    [Required] [MeetingTypeValidation] public string Type { get; set; } = "Analysis";

    public string? Description { get; set; } = string.Empty;

    public string? MeetingNotes { get; set; } = string.Empty;

    [Required] public string ClientName { get; set; } = string.Empty;

    [Required] public string UserId { get; set; } = string.Empty;
}