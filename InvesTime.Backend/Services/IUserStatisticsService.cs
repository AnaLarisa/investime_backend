using InvesTime.BackEnd.Models;

namespace InvesTime.BackEnd.Services;

public interface IUserStatisticsService
{
    Dictionary<string, Tuple<int, int>> GetAllUserStatistics();

    Dictionary<DateTime, int> GetMeetingCountByDay(string userId, string meetingType, DateTime startDate,
        DateTime endDate);

    UserStatistics? GetUserStatistics(string userId = "");
    void SetTargetNrOfClientsPerYear(int targetNrOfClients);
    void IncreaseNrOfClientsPerYear();
}