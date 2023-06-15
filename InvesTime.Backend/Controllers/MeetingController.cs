using InvesTime.BackEnd.Models;
using InvesTime.BackEnd.Models.DTO;
using InvesTime.BackEnd.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvesTime.BackEnd.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MeetingController : Controller
{
    private readonly IMeetingService _meetingService;

    public MeetingController(IMeetingService meetingService)
    {
        _meetingService = meetingService;
    }


    /// <summary>
    /// Get a list of all the meetings the current user has.
    /// </summary>
    [HttpGet(Name = "GetAllMeetings")]
    public IActionResult GetAllMeetings()
    {
        var meetings = _meetingService.GetMeetings();
        return Ok(meetings);
    }


    /// <summary>
    /// Get the first 3 upcoming meetings of the current user.
    /// </summary>
    [HttpGet("upcoming3", Name = "GetFirstThreeUpcomingMeetings")]
    public IActionResult GetFirstThreeUpcomingMeetings()
    {
        var meetings = _meetingService.GetFirstThreeUpcomingMeetings();
        return Ok(meetings);
    }


    /// <summary>
    /// Get one meeting by its id.
    /// </summary>
    [HttpGet("{id}", Name = "GetMeetingById")]
    public IActionResult GetMeetingById(string id)
    {
        var meeting = _meetingService.GetMeetingById(id);
        return Ok(meeting);
    }


    /// <summary>
    /// Add a new meeting in the personal calendar.
    /// </summary>
    /// <param name="meetingDto"></param>
    [HttpPost(Name = "AddMeeting")]
    public async Task<ActionResult<Meeting>> AddMeeting(MeetingDto meetingDto)
    {
        if (!ModelState.IsValid) return BadRequest($"One or more fields are not correct \n{meetingDto}");

        var meeting = await _meetingService.AddMeeting(meetingDto);
        return Ok(meeting);
    }


    /// <summary>
    /// Update one meeting by Id.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="updatedMeetingDto"></param>
    [HttpPut("{id}", Name = "UpdateMeeting")]
    public IActionResult UpdateMeeting(string id, [FromBody] MeetingDto updatedMeetingDto)
    {
        if (!ModelState.IsValid) return BadRequest($"One or more fields are not correct \n{updatedMeetingDto}");

        _meetingService.UpdateMeeting(id, updatedMeetingDto);

        return Ok($"Updated meeting with Id:\n{id}");
    }


    /// <summary>
    /// Delete one meeting by Id.
    /// </summary>
    /// <param name="id"></param>
    [HttpDelete("{id}", Name = "RemoveMeeting")]
    public IActionResult RemoveMeeting(string id)
    {
        _meetingService.DeleteMeeting(id);

        return Ok($"Deleted meeting with id = {id}");
    }
}