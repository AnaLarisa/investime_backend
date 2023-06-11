using InvesTime.BackEnd.Data.Repositories;
using InvesTime.BackEnd.Helpers;
using InvesTime.BackEnd.Models;
using InvesTime.BackEnd.Models.DTO;
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

        if (meeting.Type == "Consultation(C2)")
        {
            _userStatisticsService.IncreaseNrOfClientsPerYear();
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

        var result = _repository.DeleteMeeting(id);
        if (result == false)
            throw new InvalidOperationException($"Deletion operation failed for meeting with id: {id}");

        return true;
    }


    public IList<Meeting> GetMeetingsOfCurrentUser()
    {
        var userId = _userHelper.GetCurrentUserId();
        return _repository.GetMeetingsByUserId(userId);
    }

    public IList<Meeting> GetMeetingsOfMeetingTypeByConsultantUsername(string consultantUsername, string meetingType,
        DateTime startDate, DateTime endDate)
    {
        if (consultantUsername.IsNullOrEmpty()) throw new ArgumentException("Consultant username is required.");
        var consultantId = _userService.GetUserIdByUsername(consultantUsername);

        return _repository.GetMeetingsByConsultantId(consultantId, meetingType, startDate, endDate);
    }
}