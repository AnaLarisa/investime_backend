using InvesTime.BackEnd.Models;
using InvesTime.BackEnd.Models.DTO;

namespace InvesTime.BackEnd.Services;

public interface IMeetingService
{
    IEnumerable<Meeting> GetMeetings();
    Meeting GetMeetingById(string id);
    Task<Meeting> AddMeeting(MeetingDto meetingDto);
    public void UpdateMeeting(string id, MeetingDto meetingDto);
    bool DeleteMeeting(string id);
}