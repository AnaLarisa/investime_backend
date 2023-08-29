using System.ComponentModel.DataAnnotations;

namespace InvesTime.Models.DTO;

public class UserStatisticsDateRangeDto
{
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
    [Required]
    public int TargetNrOfClientsPerYear { get; set; }
    [Required]
    public int ContractsSigned { get; set; }
    [Required]
    public int ClientsCount { get; set; }

    [Required]
    public IList<Dictionary<string, int>>? MeetingsByMeetingType { get; set; }
}