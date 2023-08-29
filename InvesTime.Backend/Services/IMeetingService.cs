using InvesTime.BackEnd.Models;
using InvesTime.BackEnd.Models.DTO;

namespace InvesTime.BackEnd.Services;

public interface IMeetingService
{
    IList<Meeting> GetMeetings();
    IList<Meeting> GetFirstThreeUpcomingMeetings();
    Meeting GetMeetingById(string id);
    Task<Meeting> AddMeeting(MeetingDto meetingDto);
    public void UpdateMeeting(string id, MeetingDto meetingDto);
    bool DeleteMeeting(string id);
    IList<Dictionary<string, int>> GetMeetingsCountByUserIdDateRange(DateTime startDate, DateTime endDate, string userId="");
}