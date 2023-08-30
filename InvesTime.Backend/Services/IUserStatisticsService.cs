using InvesTime.Models.DTO;

namespace InvesTime.BackEnd.Services;

public interface IUserStatisticsService
{
    IList<string> GetGoalsListsForCurrentUser();
    void AddGoalToList(string goal);
    void RemoveGoalFromList(string goal);
    void SetTargetNrOfClientsPerYear(int targetNrOfClients);
    void IncreaseNrOfContractsSignedPerYear();
    void DecreaseNrOfContractsSignedPerYear();
    void DecreaseNrOfClientsCount();
    public void IncreaseNrOfClientsCount();
    UserStatisticsDateRangeDto GetUserStatisticsDateRangeDto(DateTime startDate, DateTime endDate, string username = "");

}