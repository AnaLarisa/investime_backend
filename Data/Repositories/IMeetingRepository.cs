using backend.Models;

namespace backend.Data.Repositories
{
    public interface IMeetingRepository
    {
        IEnumerable<Meeting> GetMeetings();
        Meeting GetMeetingById(int id);
        void AddMeeting(Meeting meeting);
        void UpdateMeeting(Meeting meeting);
        void DeleteMeeting(int id);
    }
}
