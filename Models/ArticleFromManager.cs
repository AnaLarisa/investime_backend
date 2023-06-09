using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class ArticleFromManager
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } 

    [Required] public string Title { get; set; } = "New article";

    [Required]
    [Url]
    public string Content { get; set; } = string.Empty;

    public string Observations { get; set; } = string.Empty;

}