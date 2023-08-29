using InvesTime.BackEnd.Models;

namespace InvesTime.BackEnd.Data.Repositories;

public interface IUserStatisticsRepository
{
    void AddUserStatistics(UserStatistics? userStatistics); 
    UserStatistics? GetUserStatisticsByUsername(string username);
    void UpdateUserStatistics(UserStatistics userStatistics);
}