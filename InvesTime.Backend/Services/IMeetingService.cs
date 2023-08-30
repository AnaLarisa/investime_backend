using InvesTime.BackEnd.Models;
using InvesTime.BackEnd.Models.DTO;

namespace InvesTime.BackEnd.Services;

public interface IMeetingService
{
    IList<Meeting> GetMeetings();
    IList<Meeting> GetFirstThreeUpcomingMeetings();
    Meeting GetMeetingById(string id);
    Task<Meeting> AddMeeting(MeetingDto meetingDto);
    void AddUpcomingTeamMeetingsForUser(User user);
    void UpdateMeeting(string id, MeetingDto meetingDto);
    IList<Dictionary<string, int>> GetMeetingsCountByUserIdDateRange(DateTime startDate, DateTime endDate, string userId = "");
    bool DeleteMeeting(string id);
    public bool DeleteAllMeetingsOfUserId(string userId);
    public bool DeleteTeamMeetings(Meeting meeting);
}