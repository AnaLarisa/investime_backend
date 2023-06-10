using System.ComponentModel.DataAnnotations;
using InvesTime.BackEnd.Services;

namespace InvesTime.BackEnd.Validators;

[AttributeUsage(AttributeTargets.Property)]
public class UniqueUsernameAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("The username provided is null.");
        }

        var userService = (IUserService)validationContext.GetService(typeof(IUserService))!;
        var username = (string)value;

        return userService.IsUsernameTaken(username) 
            ? new ValidationResult("Username already exists.") 
            : ValidationResult.Success!;
    }
}