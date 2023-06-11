using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace InvesTime.BackEnd.Models;

public class UserStatistics
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Username { get; set; }
    public int TargetNrOfClientsPerYear { get; set; }
    public int ClientsCount { get; set; } = 0;
}