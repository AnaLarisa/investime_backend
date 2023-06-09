using backend.Models;
using backend.Models.DTO;

namespace backend.Services;

public interface IMeetingService
{
    IEnumerable<Meeting> GetMeetings();
    Meeting GetMeetingById(string id);
    Task<Meeting> AddMeeting(MeetingDto meetingDto);
    public void UpdateMeeting(string id, MeetingDto meetingDto);
    bool DeleteMeeting(string id);
}