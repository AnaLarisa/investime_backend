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
    private UserStatistics _currentUserStatistics;

    public UserStatisticsService(IUserStatisticsRepository userStatisticsRepository,
        IMeetingRepository meetingRepository, IUserHelper userHelper, IUserRepository userRepository)
    {
        _userStatisticsRepository = userStatisticsRepository;
        _meetingRepository = meetingRepository;
        _userHelper = userHelper;
        _userRepository = userRepository;
        _currentUserStatistics = GetUserStatistics();
    }

    public IList<string> GetGoalsListsForCurrentUser()
    {
        return _currentUserStatistics.GoalsList;
    }


    public void AddGoalToList(string goal)
    {
        _currentUserStatistics.GoalsList.Add(goal);
        _userStatisticsRepository.UpdateUserStatistics(_currentUserStatistics);
    }


    public void RemoveGoalFromList(string goal)
    {
        var goalToRemove = _currentUserStatistics.GoalsList.FirstOrDefault(g => g == goal);

        if (goalToRemove != null)
        {
            _currentUserStatistics.GoalsList.Remove(goalToRemove);
            _userStatisticsRepository.UpdateUserStatistics(_currentUserStatistics);
        }
    }


    public void SetTargetNrOfClientsPerYear(int targetNrOfClients)
    {
        _currentUserStatistics.TargetNrOfClientsPerYear = targetNrOfClients;
        _userStatisticsRepository.UpdateUserStatistics(_currentUserStatistics);
    }


    public void IncreaseNrOfClientsCount()
    {
        _currentUserStatistics.ClientsCount++;
        _userStatisticsRepository.UpdateUserStatistics(_currentUserStatistics);
    }


    public void IncreaseNrOfContractsSignedPerYear()
    {
        _currentUserStatistics.ContractsSigned++;
        _userStatisticsRepository.UpdateUserStatistics(_currentUserStatistics);
    }    
    
    public void DecreaseNrOfContractsSignedPerYear()
    {
        _currentUserStatistics.ContractsSigned--;
        _userStatisticsRepository.UpdateUserStatistics(_currentUserStatistics);
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
            var newUserStatistics = new UserStatistics
            {
                Username = username
            };
            _userStatisticsRepository.AddUserStatistics(newUserStatistics);
            return newUserStatistics;
        }
        return currentUserStatistics;
    }

    public UserStatisticsDateRangeDto GetUserStatisticsDateRangeDto(DateTime startDate, DateTime endDate, string username = "")
    {
        if (username == "")
        {
            username = _userHelper.GetCurrentUserUsername();
        }

        var userId = _userRepository.GetUserByUsername(username)!.Id;
        var meet = _meetingRepository.GetMeetingsCountByUserIdDateRange(startDate, endDate, userId);


        return new UserStatisticsDateRangeDto
        {
            StartDate = startDate,
            EndDate = endDate,
            TargetNrOfClientsPerYear = _currentUserStatistics.TargetNrOfClientsPerYear,
            ContractsSigned = _currentUserStatistics.ContractsSigned,
            ClientsCount = _currentUserStatistics.ClientsCount,
            MeetingsByMeetingType = meet
        };
    }
}