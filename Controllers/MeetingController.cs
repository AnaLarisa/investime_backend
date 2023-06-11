using InvesTime.BackEnd.Models;
using InvesTime.BackEnd.Models.DTO;
using InvesTime.BackEnd.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvesTime.BackEnd.Controllers;

[Route("api/[controller]")]
[ApiController, Authorize]
public class MeetingController : Controller
{
    private readonly IMeetingService _meetingService;

    public MeetingController(IMeetingService meetingService)
    {
        _meetingService = meetingService;
    }

    [HttpGet(Name = "GetAllMeetings")]
    public IActionResult GetAllMeetings()
    {
        var meetings = _meetingService.GetMeetings();
        return Ok(meetings);
    }


    [HttpGet("upcoming3",Name = "GetFirstThreeUpcomingMeetings")]
    public IActionResult GetFirstThreeUpcomingMeetings()
    {
        var meetings = _meetingService.GetFirstThreeUpcomingMeetings();
        return Ok(meetings);
    }


    [HttpGet("{id}", Name = "GetMeetingById")]
    public IActionResult GetMeetingById(string id)
    {
        var meeting = _meetingService.GetMeetingById(id);
        return Ok(meeting);
    }


    [HttpPost(Name = "AddMeeting")]
    public async Task<ActionResult<Meeting>> AddMeeting(MeetingDto meetingDto)
    {
        if (!ModelState.IsValid) return BadRequest($"One or more fields are not correct \n{meetingDto}");

        var meeting = await _meetingService.AddMeeting(meetingDto);
        return Ok(meeting);
    }


    [HttpPut("{id}", Name = "UpdateMeeting")]
    public IActionResult UpdateMeeting(string id, [FromBody] MeetingDto updatedMeetingDto)
    {
        if (!ModelState.IsValid) return BadRequest($"One or more fields are not correct \n{updatedMeetingDto}");

        _meetingService.UpdateMeeting(id, updatedMeetingDto);

        return Ok($"Updated meeting with Id:\n{id}");
    }

    [HttpDelete("{id}", Name = "RemoveMeeting")]
    public IActionResult RemoveMeeting(string id)
    {
        _meetingService.DeleteMeeting(id);

        return Ok($"Deleted meeting with id = {id}");
    }
}