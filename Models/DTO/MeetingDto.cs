using backend.Validators;
using System.ComponentModel.DataAnnotations;

namespace backend.Models.DTO;

public class MeetingDto
{
    public string Title { get; set; } = "Meeting";

    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime Date { get; set; }

    [Required]
    [DataType(DataType.Time)]
    [RegularExpression(@"^(?:[01]\d|2[0-3]):[0-5]\d$", ErrorMessage = "Invalid time format. Please use HH:mm format.")]
    public string Time { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Duration value must be greater than zero.")]
    public int Duration { get; set; } // In minutes

    public string? Location { get; set; }

    [Required]
    [MeetingTypeValidation] 
    public string Type { get; set; } = "Internal";

    public string? Description { get; set; }

    public string? MeetingNotes { get; set; }

    [Required]
    public string ClientName { get; set; }
}