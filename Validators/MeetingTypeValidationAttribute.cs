using System.ComponentModel.DataAnnotations;

namespace backend.Validators;

[AttributeUsage(AttributeTargets.Property)]
public class MeetingTypeValidationAttribute : ValidationAttribute
{
    private static readonly List<string> ValidMeetingTypes = new()
        {
            "Discovery",
            "InvestmentPlan",
            "MutualCommitment",
            "FollowUp45Day",
            "RegularProgress",
            "Seminar",
            "Internal"
        };
    public override bool IsValid(object value)
    {
        string meetingType = value.ToString() ?? throw new InvalidOperationException("The MeetingType appears to be null.");

        return ValidMeetingTypes.Contains(meetingType);
    }
}
