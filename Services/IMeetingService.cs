using backend.Models;

namespace backend.Services;

public interface IMeetingService
{
    IEnumerable<Meeting> GetMeetings();
    Meeting GetMeetingById(string id);
    Task AddMeeting(Meeting meeting);
    void UpdateMeeting(Meeting meeting);
    void DeleteMeeting(string id);
}