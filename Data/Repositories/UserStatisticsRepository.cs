using InvesTime.BackEnd.Models;
using MongoDB.Driver;

namespace InvesTime.BackEnd.Data.Repositories;

public class UserStatisticsRepository : IUserStatisticsRepository
{
    private readonly IMongoCollection<UserStatistics?> _userStatisticsCollection;

    public UserStatisticsRepository(IMongoDatabase database)
    {
        _userStatisticsCollection = database.GetCollection<UserStatistics>("userStatistics")!;
    }

    public UserStatistics? GetUserStatisticsById(string userId)
    {
        var filter = Builders<UserStatistics>.Filter.Eq(u => u.Id, userId);
        return _userStatisticsCollection!.Find(filter).FirstOrDefault();
    }

    public List<UserStatistics?> GetAllUserStatistics()
    {
        return _userStatisticsCollection.Find(_ => true).ToList();
    }

    public void AddUserStatistics(UserStatistics? userStatistics)
    {
        _userStatisticsCollection.InsertOne(userStatistics);
    }

    public void UpdateUserStatistics(UserStatistics userStatistics)
    {
        var filter = Builders<UserStatistics>.Filter.Eq(u => u.Id, userStatistics.Id);
        _userStatisticsCollection.ReplaceOne(filter!, userStatistics);
    }
}
