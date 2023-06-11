using System.ComponentModel.DataAnnotations;
using InvesTime.BackEnd.Validators;

namespace InvesTime.BackEnd.Models.DTO;

public class ChangePasswordDto
{
    public string CurrentPassword { get; set; } = string.Empty;

    [Required] [StrongPassword] public string NewPassword { get; set; } = string.Empty;

    [Required] public string NewPasswordAgain { get; set; } = string.Empty;
}