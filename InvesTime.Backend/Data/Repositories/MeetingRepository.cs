using InvesTime.BackEnd.Models;
using InvesTime.Models;
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

    public bool DeleteMeetingById(string id)
    {
        var result = _meetings.DeleteOne(m => m.Id == id);
        return result.DeletedCount > 0;
    }

    public bool DeleteMeeting(string location, string title, string userId)
    {
        var result = _meetings.DeleteOne(m =>(m.Location == location && m.Title == title && m.UserId == userId));
        return result.DeletedCount > 0;
    }

    public bool DeleteAllMeetingsOfUserId(string userId)
    {
        var result = _meetings.DeleteMany(m => m.UserId == userId);
        return result.DeletedCount >= 0;
    }

    public IList<Meeting> GetMeetingsByUserId(string userId)
    {
        return _meetings.Find(m => m.UserId == userId).ToList();
    }

    public IList<Meeting> GetTeamMeetingsByManagerId(string managerId)
    {
        return _meetings.Find(m => (m.UserId == managerId
                                    && (m.Type == nameof(MeetingType.TeamMeeting) || 
                                        m.Type == nameof(MeetingType.TellParty) ||
                                        m.Type == nameof(MeetingType.Seminar) ||
                                        m.Type == nameof(MeetingType.Training)))).ToList();
    }

    public IList<Dictionary<string, int>> GetMeetingsCountByUserIdDateRange(DateTime startDate, DateTime endDate, string userId)
    {
        var filter = Builders<Meeting>.Filter.And(
            Builders<Meeting>.Filter.Eq(m => m.UserId, userId),
            Builders<Meeting>.Filter.Gte(m => m.Date, startDate),
            Builders<Meeting>.Filter.Lte(m => m.Date, endDate)
        );

        var meetingsList = _meetings.Find(filter).ToList();

        var result = new Dictionary<string, int>();

        foreach (MeetingType mt in Enum.GetValues(typeof(MeetingType)))
        {
            var meetingType = mt.ToString();
            var count = meetingsList.Count(m => m.Type == meetingType);
            result[meetingType] = count;
        }

        var resultList = result.Select(kv => new Dictionary<string, int> { { kv.Key, kv.Value } }).ToList();

        return resultList;
    }
}