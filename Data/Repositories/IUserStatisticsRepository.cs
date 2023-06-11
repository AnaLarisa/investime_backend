using InvesTime.BackEnd.Models;

namespace InvesTime.BackEnd.Data.Repositories;

public interface IUserStatisticsRepository
{
    void AddUserStatistics(UserStatistics? userStatistics);
    List<UserStatistics?> GetAllUserStatistics();
    UserStatistics? GetUserStatisticsById(string userId);
    void UpdateUserStatistics(UserStatistics userStatistics);
}