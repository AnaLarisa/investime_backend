using System.ComponentModel.DataAnnotations;
using InvesTime.BackEnd.Validators;

namespace InvesTime.Models.DTO;

public class UserUpdateInfoDto
{
    public string? FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
    [UniqueUsername] public string? Username { get; set; } = string.Empty; 
    public string? Email { get; set; } = string.Empty;
    public string? Address { get; set; } = string.Empty; 
    public string? PhoneNumber { get; set; } = string.Empty;
    public string? City { get; set; } = string.Empty;
    [Required] public string ManagerUsername { get; set; } = string.Empty;
}