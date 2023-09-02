using InvesTime.BackEnd.Data.Repositories;
using InvesTime.BackEnd.Helpers;
using InvesTime.BackEnd.Models;
using InvesTime.Models.DTO;

namespace InvesTime.BackEnd.Services;

public class UserStatisticsService : IUserStatisticsService
{
    private readonly IMeetingRepository _meetingRepository;
    private readonly IUserHelper _userHelper;
    private readonly IUserStatisticsRepository _userStatisticsRepository;
    private readonly IUserRepository _userRepository;

    public UserStatisticsService(IUserStatisticsRepository userStatisticsRepository,
        IMeetingRepository meetingRepository, IUserHelper userHelper, IUserRepository userRepository)
    {
        _userStatisticsRepository = userStatisticsRepository;
        _meetingRepository = meetingRepository;
        _userHelper = userHelper;
        _userRepository = userRepository;
    }

    public IList<string> GetGoalsListsForCurrentUser()
    {
        return GetUserStatistics().GoalsList;
    }


    public void AddGoalToList(string goal)
    {
        var currentUserStatistics = GetUserStatistics();
        currentUserStatistics.GoalsList.Add(goal);
        _userStatisticsRepository.UpdateUserStatistics(currentUserStatistics);
    }


    public void RemoveGoalFromList(string goal)
    {
        var goalToRemove = GetUserStatistics().GoalsList.FirstOrDefault(g => g == goal);
        var currentUserStatistics = GetUserStatistics();
        if (goalToRemove != null)
        {
            currentUserStatistics.GoalsList.Remove(goalToRemove);
            _userStatisticsRepository.UpdateUserStatistics(currentUserStatistics);
        }
    }


    public void SetTargetNrOfClientsPerYear(int targetNrOfClients)
    {
        var currentUserStatistics = GetUserStatistics();
        currentUserStatistics.TargetNrOfClientsPerYear = targetNrOfClients;
        _userStatisticsRepository.UpdateUserStatistics(currentUserStatistics);
    }


    public void IncreaseNrOfClientsCount()
    {
        var currentUserStatistics = GetUserStatistics();
        currentUserStatistics.ClientsCount++;
        _userStatisticsRepository.UpdateUserStatistics(currentUserStatistics);
    }


    public void IncreaseNrOfContractsSignedPerYear()
    {
        var currentUserStatistics = GetUserStatistics();
        currentUserStatistics.ContractsSigned++;
        _userStatisticsRepository.UpdateUserStatistics(currentUserStatistics);
    }    
    
    public void DecreaseNrOfContractsSignedPerYear()
    {
        var currentUserStatistics = GetUserStatistics();
        currentUserStatistics.ContractsSigned--;
        _userStatisticsRepository.UpdateUserStatistics(currentUserStatistics);
    }

    public void DecreaseNrOfClientsCount()
    {
        var currentUserStatistics = GetUserStatistics();
        currentUserStatistics.ClientsCount--;
        _userStatisticsRepository.UpdateUserStatistics(currentUserStatistics);
    }

    private UserStatistics GetUserStatistics(string username = "")
    {
        if (username == "")
        {
            username = _userHelper.GetCurrentUserUsername();
        }

        var currentUserStatistics = _userStatisticsRepository.GetUserStatisticsByUsername(username);
        if (currentUserStatistics == null)
        {
            currentUserStatistics = new UserStatistics
            {
                Username = username
            };

            _userStatisticsRepository.AddUserStatistics(currentUserStatistics);
            var userStatisticsAdded = _userStatisticsRepository.GetUserStatisticsByUsername(username);
            if (userStatisticsAdded != null)
                return userStatisticsAdded;
        }
        return currentUserStatistics;
    }

    public UserStatisticsDateRangeDto GetUserStatisticsDateRangeDto(DateTime startDate, DateTime endDate, string username = "")
    {
        if (username == "")
        {
            username = _userHelper.GetCurrentUserUsername();
        }

        var userId = _userRepository.GetUserByUsername(username).Id;
        var meetingsByMeetingType = _meetingRepository.GetMeetingsCountByUserIdDateRange(startDate, endDate, userId);
        var currentUserStatistics = GetUserStatistics();

        return new UserStatisticsDateRangeDto
        {
            StartDate = startDate,
            EndDate = endDate,
            TargetNrOfClientsPerYear = currentUserStatistics.TargetNrOfClientsPerYear,
            ContractsSigned = currentUserStatistics.ContractsSigned,
            ClientsCount = currentUserStatistics.ClientsCount,
            MeetingsByMeetingType = meetingsByMeetingType
        };
    }
}