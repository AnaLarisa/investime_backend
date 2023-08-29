using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InvesTime.BackEnd.Models;

public class UserStatistics
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    [Required] public string ConsultantUsername { get; set; } = string.Empty;
    public int TargetNrOfClientsPerYear { get; set; } = 0;
    public int ContractsSigned { get; set; } = 0;
    public int ClientsCount { get; set; } = 0;
    public DateTime UserStatisticsCreatedOn { get; set; } = DateTime.Now;
    public IList<string> GoalsList { get; set; } = new List<string>();
}