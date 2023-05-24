using backend.Models;

namespace backend.Data.Repositories
{
    public interface IMeetingRepository
    {
        IEnumerable<Meeting> GetMeetings();
        Meeting GetMeetingById(Guid id);
        void AddMeeting(Meeting meeting);
        void UpdateMeeting(Meeting meeting);
        void DeleteMeeting(Guid id);
    }
}
