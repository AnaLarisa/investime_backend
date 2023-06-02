using backend.Models;
using MongoDB.Driver;

namespace backend.Data.Repositories;

public class MeetingRepository : IMeetingRepository
{
    private readonly IMongoCollection<Meeting> _meetings;

    public MeetingRepository(IMongoDatabase database)
    {
        _meetings = database.GetCollection<Meeting>("meetings");
    }

    public IEnumerable<Meeting> GetMeetings()
    {
        return _meetings.Find(meeting => true).ToList();
    }

    public Meeting GetMeetingById(string id)
    {
        return _meetings.Find(meeting => meeting.Id == id).FirstOrDefault();
    }

    public async Task AddMeeting(Meeting meeting)
    {
        await _meetings.InsertOneAsync(meeting);
    }

    public void UpdateMeeting(Meeting meeting)
    {
        _meetings.ReplaceOne(m => m.Id == meeting.Id, meeting);
    }

    public void DeleteMeeting(string id)
    {
        _meetings.DeleteOne(m => m.Id == id);
    }
}