using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace backend.Models.DTO;

public class UserDto
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}