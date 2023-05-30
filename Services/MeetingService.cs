using backend.Data.Repositories;
using backend.Models;

namespace backend.Services
{
    public class MeetingService : IMeetingService
    {
        private readonly IMeetingRepository _repository;

        public MeetingService(IMeetingRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Meeting> GetMeetings()
        {
            return _repository.GetMeetings();
        }

        public Meeting GetMeetingById(int id)
        {
            return _repository.GetMeetingById(id);
        }

        public void AddMeeting(Meeting meeting)
        {
            _repository.AddMeeting(meeting);
        }

        public void UpdateMeeting(Meeting meeting)
        {
            _repository.UpdateMeeting(meeting);
        }

        public void DeleteMeeting(int id)
        {
            _repository.DeleteMeeting(id);
        }
    }
}