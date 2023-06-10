using System.ComponentModel.DataAnnotations;
using InvesTime.BackEnd.Validators;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InvesTime.BackEnd.Models;

public class RegistrationRequest
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required] 
    public string LastName { get; set; } = string.Empty;

    [Required]
    [UniqueUsername]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string ManagerUsername { get; set; } = string.Empty;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [DataType(DataType.DateTime)]
    public DateTime DateTime { get; set; } = DateTime.UtcNow;
}