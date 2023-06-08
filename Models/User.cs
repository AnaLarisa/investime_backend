using backend.Validators;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    [Required]
    public string Username { get; set; } = string.Empty;

    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }

    public bool IsAdmin { get; set; } = false;

    [Required]
    public string ManagerUsername { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public bool EmailConfirmed { get; set; } = false;
    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    public bool MeetingsNotificationsOff { get; set; } = false;

    [MaxImageSize(2 * 1024 * 1024)]
    public byte[] Image { get; set; } = GetDefaultImage();


    private static byte[] GetDefaultImage()
    {
        string defaultImagePath = "default-image.png";
        byte[] defaultImageBytes = File.ReadAllBytes(defaultImagePath);

        return defaultImageBytes;
    }
}