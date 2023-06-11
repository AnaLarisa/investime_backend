using System.ComponentModel.DataAnnotations;

namespace InvesTime.BackEnd.Validators;

public class MaxImageSizeAttribute : ValidationAttribute
{
    private readonly int _maxSizeInBytes;

    public MaxImageSizeAttribute(int maxSizeInBytes)
    {
        _maxSizeInBytes = maxSizeInBytes;
    }

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null) return new ValidationResult("The image provided is null.");

        if (value is byte[] imageBytes && imageBytes.Length > _maxSizeInBytes)
            return new ValidationResult($"Image size should not exceed {_maxSizeInBytes} bytes.");

        return ValidationResult.Success!;
    }
}