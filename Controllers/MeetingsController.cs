using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace backend.Controllers;

[Route("api/[controller]")]
[ApiController, Authorize(Roles = "User")]
public class MeetingsController : Controller
{
    private readonly IMeetingService _meetingService;

    public MeetingsController(IMeetingService meetingService)
    {
        _meetingService = meetingService;
    }

    [HttpGet(Name = "GetAllMeetings"), Authorize(Roles = "Admin")]
    public IActionResult GetAllMeetings()
    {
        var meetings = _meetingService.GetMeetings();
        return Ok(meetings);
    }


    [HttpGet("{id}", Name = "GetMeetingById")]
    public IActionResult GetMeetingById(string id)
    {
        var meeting = _meetingService.GetMeetingById(id);
        return Ok(meeting);
    }


    [HttpPost(Name = "AddMeeting")]
    public async Task<IActionResult> AddMeeting(Meeting meeting)
    {
        if (!ModelState.IsValid) return BadRequest($"One or more fields are not correct \n{meeting}");

        await _meetingService.AddMeeting(meeting);
        return Ok($"Added meeting with id = {meeting.Id}");
    }


    [HttpPut("{id}", Name = "UpdateMeeting")]
    public IActionResult UpdateMeeting(string id, [FromBody] Meeting updatedMeeting)
    {
        if (!ModelState.IsValid) return BadRequest($"One or more fields are not correct \n{updatedMeeting}");
        
        updatedMeeting.Id = id;
        _meetingService.UpdateMeeting(updatedMeeting);

        return Ok($"Updated meeting with Id:\n{updatedMeeting.Id}");
    }

    [HttpDelete("{id}", Name = "RemoveMeeting")]
    public IActionResult RemoveMeeting(string id)
    {
        _meetingService.DeleteMeeting(id);

        return Ok($"Deleted meeting with id = {id}");
    }
}