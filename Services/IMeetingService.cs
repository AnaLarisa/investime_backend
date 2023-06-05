using backend.Models;

namespace backend.Services;

public interface IMeetingService
{
    IEnumerable<Meeting> GetMeetings();
    Meeting GetMeetingById(string id);
    Task<Meeting> AddMeeting(MeetingDto meetingDto);
    public void UpdateMeeting(string id, MeetingDto meetingDto);
    void DeleteMeeting(string id);
}