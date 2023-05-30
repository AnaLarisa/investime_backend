using backend.Models;

namespace backend.Services;

public interface IMeetingService
{
    IEnumerable<Meeting> GetMeetings();
    Meeting GetMeetingById(int id);
    void AddMeeting(Meeting meeting);
    void UpdateMeeting(Meeting meeting);
    void DeleteMeeting(int id);
}