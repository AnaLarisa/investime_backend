using backend.Data.Repositories;
using backend.Helpers;
using backend.Models;
using backend.Models.DTO;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace backend.Services;

public class MeetingService : IMeetingService
{
    private readonly IMeetingRepository _repository;
    private IUserService _userService;

    public MeetingService(IMeetingRepository repository, IUserService userService)
    {
        _repository = repository;
        _userService = userService;
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

        var result  = _repository.GetMeetingById(id);

        return result == null
            ? throw new InvalidOperationException($"The meeting with id = {id} was not found")
            : result;
    }

    public async Task<Meeting> AddMeeting(MeetingDto meetingDto)
    {
        var meeting = ObjectConverter.Convert<MeetingDto, Meeting>(meetingDto);
        meeting.UserId = _userService.GetCurrentUserId()!;
        await _repository.AddMeeting(meeting);

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
        if (id.IsNullOrEmpty())
        {
            throw new ArgumentException("Meeting ID is required.");
        }

        var result = _repository.DeleteMeeting(id);
        if (result == false)
        {
            throw new InvalidOperationException($"Deletion operation failed for meeting with id: {id}");
        }

        return true;
    }
}