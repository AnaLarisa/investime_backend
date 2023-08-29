using System.ComponentModel.DataAnnotations;
using InvesTime.BackEnd.Validators;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InvesTime.BackEnd.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [Required] public string FirstName { get; set; } = string.Empty;

    [Required] public string LastName { get; set; } = string.Empty;

    [Required] [UniqueUsername] public string Username { get; set; } = string.Empty;

    public byte[] PasswordHash { get; set; } = new byte[32];
    public byte[] PasswordSalt { get; set; } = new byte[32];

    public bool IsAdmin { get; set; } = false;

    [Required] public string ManagerUsername { get; set; } = string.Empty;

    [Required] [EmailAddress] public string Email { get; set; } = string.Empty;

    public bool EmailConfirmed { get; set; } = false;

    [Phone] public string PhoneNumber { get; set; } = string.Empty;

    public bool MeetingsNotificationsOff { get; set; } = false;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
}