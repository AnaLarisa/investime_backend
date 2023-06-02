using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MeetingsController : Controller
{
    private readonly IMeetingService _meetingService;

    public MeetingsController(IMeetingService meetingService)
    {
        _meetingService = meetingService;
    }

    [HttpGet(Name = "GetAllMeetings")]
    public IActionResult GetAllMeetings()
    {
        var meetings = _meetingService.GetMeetings();
        return Ok(meetings);
    }

    [HttpPost(Name = "AddMeeting")]
    public async Task<IActionResult> AddMeeting(Meeting meeting)
    {
        if (ModelState.IsValid)
        {
            await _meetingService.AddMeeting(meeting);
            return Ok($"Added meeting with id = {meeting.Id}");
        }

        // If the model is not valid, return the create view with the model to show validation errors
        return Ok(meeting);
    }

    [HttpDelete("{id}", Name = "RemoveMeeting")]
    public IActionResult RemoveMeeting(string id)
    {
        _meetingService.DeleteMeeting(id);

        return Ok($"Deleted meeting with id = {id}");
    }
}