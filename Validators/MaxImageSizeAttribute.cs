using System.ComponentModel.DataAnnotations;

namespace backend.Validators;

public class MaxImageSizeAttribute : ValidationAttribute
{
    private readonly int _maxSizeInBytes;

    public MaxImageSizeAttribute(int maxSizeInBytes)
    {
        this._maxSizeInBytes = maxSizeInBytes;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is byte[] imageBytes && imageBytes.Length > _maxSizeInBytes)
        {
            return new ValidationResult($"Image size should not exceed {_maxSizeInBytes} bytes.");
        }

        return ValidationResult.Success!;
    }
}
