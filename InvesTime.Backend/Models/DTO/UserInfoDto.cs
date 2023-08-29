using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace InvesTime.Models.DTO;

public class UserInfoDto
{
    [BsonId]
    public string Id { get; set; }

    [Required] public string AuthorizationToken { get; set; } = string.Empty;

    [Required] public string FirstName { get; set; } = string.Empty;

    [Required] public string LastName { get; set; } = string.Empty;

    [Required] public string Username { get; set; } = string.Empty;

    public bool IsAdmin { get; set; } = false;

    [Required] public string ManagerUsername { get; set; } = string.Empty;

    [Required][EmailAddress] public string Email { get; set; } = string.Empty;

    [Phone] public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;

    public bool EmailConfirmed { get; set; } = false;

    public bool MeetingsNotificationsOff { get; set; } = false;
}