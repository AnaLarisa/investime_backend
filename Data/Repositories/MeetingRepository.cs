using backend.Models;
using MongoDB.Bson;
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

    public void UpdateMeeting(Meeting updatedMeeting)
    {
        var objectId = new ObjectId(updatedMeeting.Id);
        var filter = Builders<Meeting>.Filter.Eq("_id", objectId);
        _meetings.ReplaceOne(filter, updatedMeeting);
    }

    public void DeleteMeeting(string id)
    {
        _meetings.DeleteOne(m => m.Id == id);
    }
}