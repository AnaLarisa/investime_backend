using backend.Models;
using backend.Services;
using MongoDB.Bson;
using MongoDB.Driver;

namespace backend.Data.Repositories
{
    public class MeetingRepository : IMeetingRepository
    {
        private readonly IMongoCollection<Meeting> _meetings;

        public MeetingRepository(DatabaseManager databaseManager)
        {
            var database = databaseManager.GetDatabase();
            _meetings = database.GetCollection<Meeting>("meetings");
        }

        public IEnumerable<Meeting> GetMeetings()
        {
            return _meetings.Find(meeting => true).ToList();
        }

        public Meeting GetMeetingById(int id)
        {
            return _meetings.Find(meeting => meeting.Id == id).FirstOrDefault();
        }

        public void AddMeeting(Meeting meeting)
        {
            _meetings.InsertOne(meeting);
        }

        public void UpdateMeeting(Meeting meeting)
        {
            _meetings.ReplaceOne(m => m.Id == meeting.Id, meeting);
        }

        public void DeleteMeeting(int id)
        {
            _meetings.DeleteOne(m => m.Id == id);
        }
    }

}
