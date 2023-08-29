using InvesTime.Models;
using System.ComponentModel.DataAnnotations;

namespace InvesTime.BackEnd.Validators;

[AttributeUsage(AttributeTargets.Property)]
public class MeetingTypeValidationAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value != null && Enum.TryParse(value.ToString(), out MeetingType meetingType))
        {
            return Enum.IsDefined(typeof(MeetingType), meetingType);
        }

        throw new InvalidOperationException("The MeetingType is not a valid enum value. -- ");
    }
}