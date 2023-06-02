using backend.Data.Repositories;
using backend.Models;

namespace backend.Services;

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

    public Meeting GetMeetingById(string id)
    {
        return _repository.GetMeetingById(id);
    }

    public async Task AddMeeting(Meeting meeting)
    {
        await _repository.AddMeeting(meeting);
    }

    public void UpdateMeeting(Meeting meeting)
    {
        _repository.UpdateMeeting(meeting);
    }

    public void DeleteMeeting(string id)
    {
        _repository.DeleteMeeting(id);
    }
}