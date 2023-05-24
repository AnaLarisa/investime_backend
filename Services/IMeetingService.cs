using backend.Models;

namespace backend.Services;

public interface IMeetingService
{
    IEnumerable<Meeting> GetMeetings();
    Meeting GetMeetingById(Guid id);
    void AddMeeting(Meeting meeting);
    void UpdateMeeting(Meeting meeting);
    void DeleteMeeting(Guid id);
}