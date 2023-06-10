using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace InvesTime.BackEnd.Validators;

public class StrongPasswordAttribute : ValidationAttribute
{
    private new const string ErrorMessage = "The password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.";

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("The password provided is null.");
        }

        if (value is string password && !IsStrongPassword(password))
        {
            return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success!;
    }

    private static bool IsStrongPassword(string password)
    {
        const string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$";
        return Regex.IsMatch(password, pattern);
    }
}