using InvesTime.BackEnd.Models;

namespace InvesTime.BackEnd.Data.Repositories;

public interface IMeetingRepository
{
    IEnumerable<Meeting> GetMeetings();
    Meeting GetMeetingById(string id);
    Task AddMeeting(Meeting meeting);
    void UpdateMeeting(Meeting updatedMeeting);
    bool DeleteMeeting(string id);
    bool DeleteAllMeetingsOfUserId(string userId);
    IList<Meeting> GetMeetingsByUserId(string userId);

    IList<Meeting> GetMeetingsByConsultantId(string consultantId, string meetingType, DateTime startDate,
        DateTime endDate);
}