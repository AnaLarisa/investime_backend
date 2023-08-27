using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InvesTime.BackEnd.Models;

public class ArticleFromManager
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [Required] public string Title { get; set; } = "New article";

    [Required] public string Content { get; set; }

    public string Observations { get; set; } = string.Empty;
}