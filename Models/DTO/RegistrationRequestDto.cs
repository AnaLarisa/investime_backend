using System.ComponentModel.DataAnnotations;
using InvesTime.BackEnd.Validators;

namespace InvesTime.BackEnd.Models.DTO;

public class RegistrationRequestDto
{
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
}