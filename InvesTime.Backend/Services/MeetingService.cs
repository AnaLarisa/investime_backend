using InvesTime.BackEnd.Data.Repositories;
using InvesTime.BackEnd.Helpers;
using InvesTime.BackEnd.Models;
using InvesTime.BackEnd.Models.DTO;
using InvesTime.Models;
using Microsoft.IdentityModel.Tokens;

namespace InvesTime.BackEnd.Services;

public class MeetingService : IMeetingService
{
    private readonly IMeetingRepository _repository;
    private readonly IUserHelper _userHelper;
    private readonly IUserService _userService;
    private readonly IUserStatisticsService _userStatisticsService;

    public MeetingService(IMeetingRepository repository, IUserService userService, IUserHelper userHelper,
        IUserStatisticsService userStatisticsService)
    {
        _repository = repository;
        _userService = userService;
        _userHelper = userHelper;
        _userStatisticsService = userStatisticsService;
    }


    public IList<Meeting> GetMeetings()
    {
        var userId = _userHelper.GetCurrentUserId();
        return userId.IsNullOrEmpty()
            ? new List<Meeting>()
            : _repository.GetMeetingsByUserId(userId);
    }

    public IList<Meeting> GetFirstThreeUpcomingMeetings()
    {
        var userId = _userHelper.GetCurrentUserId();
        return userId.IsNullOrEmpty()
            ? new List<Meeting>()
            : _repository.GetMeetingsByUserId(userId).Where(m => m.Date >= DateTime.Now).OrderBy(m => m.Date).Take(3)
                .ToList();
    }


    public Meeting GetMeetingById(string id)
    {
        if (id.IsNullOrEmpty()) throw new ArgumentException("Meeting ID is required.");

        var result = _repository.GetMeetingById(id);

        return result == null
            ? throw new InvalidOperationException($"The meeting with id = {id} was not found")
            : result;
    }


    public async Task<Meeting> AddMeeting(MeetingDto meetingDto)
    {
        var meeting = ObjectConverter.Convert<MeetingDto, Meeting>(meetingDto);
        meeting.UserId = _userHelper.GetCurrentUserId();
        await _repository.AddMeeting(meeting);

        if (meeting.Type == nameof(MeetingType.ConsultationC2))
        {
            _userStatisticsService.IncreaseNrOfContractsSignedPerYear();
        }
        else if (meeting.Type == nameof(MeetingType.ConsultationC1))
        {
            _userStatisticsService.IncreaseNrOfClientsCount();
        }

        return meeting;
    }


    public void UpdateMeeting(string id, MeetingDto meetingDto)
    {
        var updatedMeeting = ObjectConverter.Convert<MeetingDto, Meeting>(meetingDto);
        var currentMeeting = GetMeetingById(id);
        updatedMeeting.Id = currentMeeting.Id;
        updatedMeeting.UserId = currentMeeting.UserId;

        _repository.UpdateMeeting(updatedMeeting);
    }


    public bool DeleteMeeting(string id)
    {
        if (id.IsNullOrEmpty()) throw new ArgumentException("Meeting ID is required.");
        var meeting = _repository.GetMeetingById(id);

        var result = _repository.DeleteMeeting(id);
        if (result == false)
            throw new InvalidOperationException($"Deletion operation failed for meeting with id: {id}");

        if (meeting.Type == nameof(MeetingType.ConsultationC2))
            _userStatisticsService.DecreaseNrOfContractsSignedPerYear();

        return true;
    }

    public IList<Dictionary<string, int>> GetMeetingsCountByUserIdDateRange(DateTime startDate, DateTime endDate, string userId = "")
    {
        if (userId.IsNullOrEmpty())
        {
            userId = _userHelper.GetCurrentUserId();
        }

        return _repository.GetMeetingsCountByUserIdDateRange(startDate, endDate, userId);
    }
}