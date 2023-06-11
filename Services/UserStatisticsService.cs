using InvesTime.BackEnd.Data.Repositories;
using InvesTime.BackEnd.Helpers;
using InvesTime.BackEnd.Models;

namespace InvesTime.BackEnd.Services;

public class UserStatisticsService : IUserStatisticsService
{
    private readonly IUserStatisticsRepository _userStatisticsRepository;
    private readonly IMeetingRepository _meetingRepository;
    private readonly IUserHelper _userHelper;

    public UserStatisticsService(IUserStatisticsRepository userStatisticsRepository, IMeetingRepository meetingRepository, IUserHelper userHelper)
    {
        _userStatisticsRepository = userStatisticsRepository;
        _meetingRepository = meetingRepository;
        _userHelper = userHelper;
    }

    public void SetTargetNrOfClientsPerYear(int targetNrOfClients)
    {
        var userId = _userHelper.GetCurrentUserId();
        var username = _userHelper.GetCurrentUserUsername();
        var userStatistics = _userStatisticsRepository.GetUserStatisticsById(userId);

        if (userStatistics == null)
        {
            userStatistics = new UserStatistics
            {
                Id = userId, 
                Username = username,
                TargetNrOfClientsPerYear = targetNrOfClients
            };
            _userStatisticsRepository.AddUserStatistics(userStatistics);
        }
        else
        {
            userStatistics.TargetNrOfClientsPerYear = targetNrOfClients;
            _userStatisticsRepository.UpdateUserStatistics(userStatistics);
        }
    }

    public void IncreaseNrOfClientsPerYear()
    {
        var userId = _userHelper.GetCurrentUserId();
        var username = _userHelper.GetCurrentUserUsername();
        var userStatistics = _userStatisticsRepository.GetUserStatisticsById(userId);

        if (userStatistics == null)
        {
            userStatistics = new UserStatistics
            {
                Id = userId,
                Username = username,
                TargetNrOfClientsPerYear = 0,
                ClientsCount = 1
            };
            _userStatisticsRepository.AddUserStatistics(userStatistics);
        }
        else
        {
            userStatistics.ClientsCount++;
            _userStatisticsRepository.UpdateUserStatistics(userStatistics);
        }
    }

    public Dictionary<DateTime, int> GetMeetingCountByDay(string userId, string meetingType, DateTime startDate, DateTime endDate)
    {
        var meetings = _meetingRepository.GetMeetingsByUserId(userId);

        var meetingCountByDay = new Dictionary<DateTime, int>();

        foreach (var meeting in meetings)
        {
            if (meeting.Type == meetingType && meeting.Date >= startDate && meeting.Date <= endDate)
            {
                var day = meeting.Date.Date;
                if (!meetingCountByDay.ContainsKey(day))
                {
                    meetingCountByDay[day] = 0;
                }

                meetingCountByDay[day]++;
            }
        }

        return meetingCountByDay;
    }

    public UserStatistics? GetUserStatistics(string userId = "")
    {
        if (userId == "")
        {
            userId = _userHelper.GetCurrentUserId();
        }

        return _userStatisticsRepository.GetUserStatisticsById(userId);
    }

    public Dictionary<string, Tuple<int, int>> GetAllUserStatistics()
    {
        var userStatisticsDictionary = new Dictionary<string, Tuple<int, int>>();

        var allUserStatistics = _userStatisticsRepository.GetAllUserStatistics();

        foreach (var userStatistics in allUserStatistics)
        {
            userStatisticsDictionary[userStatistics!.Username] = Tuple.Create(userStatistics.TargetNrOfClientsPerYear, userStatistics.ClientsCount);
        }

        return userStatisticsDictionary;
    }
}