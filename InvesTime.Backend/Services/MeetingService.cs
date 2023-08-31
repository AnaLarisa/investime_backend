using InvesTime.BackEnd.Data.Repositories;
using InvesTime.BackEnd.Helpers;
using InvesTime.BackEnd.Models;
using InvesTime.BackEnd.Models.DTO;
using InvesTime.Models;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;

namespace InvesTime.BackEnd.Services;

public class MeetingService : IMeetingService
{
    private readonly IMeetingRepository _repository;
    private readonly IUserHelper _userHelper;
    private readonly IUserRepository _userRepository;
    private readonly IUserStatisticsService _userStatisticsService;

    public MeetingService(IMeetingRepository repository, IUserRepository userRepository, IUserHelper userHelper,
        IUserStatisticsService userStatisticsService)
    {
        _repository = repository;
        _userHelper = userHelper;
        _userRepository = userRepository;
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
        meeting.Id = new BsonObjectId(ObjectId.GenerateNewId()).ToString();

        if (IsAdminAddAllowed(meeting))
        {
            await AddMeetingsForAdmin(meeting);
        }
        else
        {
            if (meeting.Type == nameof(MeetingType.ConsultationC2))
            {
                _userStatisticsService.IncreaseNrOfContractsSignedPerYear();
            }
            else if (meeting.Type == nameof(MeetingType.ConsultationC1))
            {
                _userStatisticsService.IncreaseNrOfClientsCount();
            }
        }

        return meeting;
    }

    private bool IsAdminAddAllowed(Meeting meeting)
    {
        return _userHelper.IsCurrentUserAdmin() &&
               (meeting.Type == nameof(MeetingType.TeamMeeting) ||
                meeting.Type == nameof(MeetingType.TellParty) ||
                meeting.Type == nameof(MeetingType.Seminar) ||
                meeting.Type == nameof(MeetingType.Training));
    }

    private async Task AddMeetingsForAdmin(Meeting meeting)
    {
        if (IsAdminAddAllowed(meeting))
        {
            var managerUsername = _userHelper.GetCurrentUserUsername();
            var consultants = _userRepository.GetAllConsultantUsernamesUnderManager(managerUsername);
            await _repository.AddMeeting(meeting);

            foreach (var consultant in consultants)
            {
                meeting.UserId = consultant.Id;
                meeting.Id = new BsonObjectId(ObjectId.GenerateNewId()).ToString();
                await _repository.AddMeeting(meeting);
            }
        }
    }



    public void AddUpcomingTeamMeetingsForUser(User user)
    {
        var managerId = _userHelper.GetCurrentUserId();

        var meetings = _repository.GetTeamMeetingsByManagerId(managerId).Where(m => m.Date >= user.CreatedOn).ToList();
        if (meetings.Count > 0)
        {
            foreach (var meeting in meetings)
            {
                meeting.UserId = user.Id;
                _repository.AddMeeting(meeting);
            }
        }
    }


    public void UpdateMeeting(string id, MeetingDto meetingDto)
    {
        var updatedMeeting = ObjectConverter.Convert<MeetingDto, Meeting>(meetingDto);
        var currentMeeting = GetMeetingById(id);
        updatedMeeting.Id = currentMeeting.Id;
        updatedMeeting.UserId = currentMeeting.UserId;

        _repository.UpdateMeeting(updatedMeeting);
    }

    public IList<Dictionary<string, int>> GetMeetingsCountByUserIdDateRange(DateTime startDate, DateTime endDate, string userId = "")
    {
        if (userId.IsNullOrEmpty())
        {
            userId = _userHelper.GetCurrentUserId();
        }

        return _repository.GetMeetingsCountByUserIdDateRange(startDate, endDate, userId);
    }


    public bool DeleteMeeting(string id)
    {
        if (id.IsNullOrEmpty())
            throw new ArgumentException("Meeting ID is required.");

        var meeting = _repository.GetMeetingById(id);

        if (meeting.UserId != _userHelper.GetCurrentUserId())
            throw new SecurityTokenException("You are not allowed to delete this meeting.");

        if (_userHelper.IsCurrentUserAdmin())
        {
            if (!DeleteTeamMeetings(meeting))
                throw new SecurityTokenException("You are not allowed to delete this meeting.");
        }

        var result = _repository.DeleteMeetingById(id);

        if (!result)
            throw new InvalidOperationException($"Deletion operation failed for meeting with id: {id}");

        if (meeting.Type == nameof(MeetingType.ConsultationC2))
            _userStatisticsService.DecreaseNrOfContractsSignedPerYear();
        else if (meeting.Type == nameof(MeetingType.ConsultationC1))
            _userStatisticsService.DecreaseNrOfClientsCount();

        return true;
    }


    public bool DeleteAllMeetingsOfUserId(string userId)
    {
        if (userId.IsNullOrEmpty()) throw new ArgumentException("User ID is required.");

        var result = _repository.DeleteAllMeetingsOfUserId(userId);
        if (result == false)
            throw new InvalidOperationException($"Deletion operation failed for meetings of user with id: {userId}");

        return true;
    }

    public bool DeleteTeamMeetings(Meeting meeting)
    {
        var managerId = _userHelper.GetCurrentUserId();
        if (_repository.GetTeamMeetingsByManagerId(managerId).Contains(meeting))
        {
            var managerUsername = _userHelper.GetCurrentUserUsername();
            var consultants = _userRepository.GetAllConsultantUsernamesUnderManager(managerUsername);
            foreach (var c in consultants)
            {
                meeting.UserId = c.Id;
                if (!_repository.DeleteMeeting(meeting.Location, meeting.Title, c.Id))
                {
                    return false;
                }
            }
        }
        return true;
    }
}