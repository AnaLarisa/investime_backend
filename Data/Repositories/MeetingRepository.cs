using InvesTime.BackEnd.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace InvesTime.BackEnd.Data.Repositories;

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

    public bool DeleteMeeting(string id)
    {
        var result = _meetings.DeleteOne(m => m.Id == id);
        return result.DeletedCount > 0;
    }

    public bool DeleteAllMeetingsOfUserId(string userId)
    {
        var result = _meetings.DeleteMany(m => m.UserId == userId);
        return result.DeletedCount > 0;
    }

    public IList<Meeting> GetMeetingsByUserId(string userId)
    {
        return _meetings.Find(m => m.UserId == userId).ToList();
    }

    public IList<Meeting> GetMeetingsByConsultantId(string consultantId, string meetingType, DateTime startDate, DateTime endDate)
    {
        var result = _meetings.Find(meeting => meeting.UserId == consultantId &&
                                               meeting.Type == meetingType &&
                                               meeting.Date >= startDate &&
                                               meeting.Date <= endDate).ToList();
        return result ?? new List<Meeting>();
    }
}