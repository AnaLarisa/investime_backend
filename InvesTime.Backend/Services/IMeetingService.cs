using InvesTime.BackEnd.Models;
using InvesTime.BackEnd.Models.DTO;

namespace InvesTime.BackEnd.Services;

public interface IMeetingService
{
    IList<Meeting> GetMeetings();
    IList<Meeting> GetFirstThreeUpcomingMeetings();
    Meeting GetMeetingById(string id);
    Dictionary<string, IList<Meeting>> GetMeetingsOfUserIdSortedByType(string userId = "");

    IList<Meeting> GetMeetingsOfMeetingTypeByConsultantUsername(string consultantUsername, string meetingType,
        DateTime startDate, DateTime endDate);

    Task<Meeting> AddMeeting(MeetingDto meetingDto);
    public void UpdateMeeting(string id, MeetingDto meetingDto);
    bool DeleteMeeting(string id);
}