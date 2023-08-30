using InvesTime.BackEnd.Models;

namespace InvesTime.BackEnd.Data.Repositories;

public interface IMeetingRepository
{
    Meeting GetMeetingById(string id);
    Task AddMeeting(Meeting meeting);
    void UpdateMeeting(Meeting updatedMeeting);
    bool DeleteMeetingById(string id);
    bool DeleteMeeting(string location, string title, string userId);
    bool DeleteAllMeetingsOfUserId(string userId);
    IList<Meeting> GetMeetingsByUserId(string userId);
    IList<Meeting> GetTeamMeetingsByManagerId(string managerId);
    IList<Dictionary<string, int>> GetMeetingsCountByUserIdDateRange(DateTime startDate, DateTime endDate, string userId);
}