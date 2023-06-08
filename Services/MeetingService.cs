using backend.Data.Repositories;
using backend.Models;
using backend.Models.DTO;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace backend.Services;

public class MeetingService : IMeetingService
{
    private readonly IMeetingRepository _repository;
    private IAuthService _authService;

    public MeetingService(IMeetingRepository repository, IAuthService authService)
    {
        _repository = repository;
        _authService = authService;
    }

    public IEnumerable<Meeting> GetMeetings()
    {
        return _repository.GetMeetings();
    }

    public Meeting GetMeetingById(string id)
    {
        if (id.IsNullOrEmpty())
        {
            throw new ArgumentException("Meeting ID is required.");
        }
        return _repository.GetMeetingById(id);
    }

    public async Task<Meeting> AddMeeting(MeetingDto meetingDto)
    {
        var meeting = CreateMeeting(meetingDto);
        await _repository.AddMeeting(meeting);

        return meeting;
    }

    public void UpdateMeeting(string id, MeetingDto meetingDto)
    {
        var updatedMeeting = CreateMeeting(meetingDto);
        var currentMeeting = GetMeetingById(id);
        updatedMeeting.Id = currentMeeting.Id;
        updatedMeeting.UserId = currentMeeting.UserId;

        _repository.UpdateMeeting(updatedMeeting);
    }

    public void DeleteMeeting(string id)
    {
        _repository.DeleteMeeting(id);
    }


    private Meeting CreateMeeting(MeetingDto meetingDto)
    {
        return new Meeting
        {
            Title = meetingDto.Title,
            Date = meetingDto.Date,
            Time = meetingDto.Time,
            Duration = meetingDto.Duration,
            Location = meetingDto.Location,
            Type = meetingDto.Type,
            Description = meetingDto.Description,
            MeetingNotes = meetingDto.MeetingNotes,
            ClientName = meetingDto.ClientName,
            UserId = _authService.GetCurrentUserId()!
        };
    }
}