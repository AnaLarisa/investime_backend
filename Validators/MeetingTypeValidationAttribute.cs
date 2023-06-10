using System.ComponentModel.DataAnnotations;

namespace InvesTime.BackEnd.Validators;

[AttributeUsage(AttributeTargets.Property)]
public class MeetingTypeValidationAttribute : ValidationAttribute
{
    private static readonly List<string> ValidMeetingTypes = new()
    {
        "Analysis",
        "Consultation (C1)",
        "Consultation (C2)",
        "Service",
        "Personal Meeting",
        "Team Meeting",
        "Tell party",
        "Seminar",
        "Training"
    };

    public override bool IsValid(object? value)
    {
        if (value != null)
        {
            var meetingType = value.ToString();

            return ValidMeetingTypes.Contains(meetingType!);
        }
        throw new InvalidOperationException("The MeetingType appears to be null.");
    }
}
