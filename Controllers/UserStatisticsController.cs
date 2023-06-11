using InvesTime.BackEnd.Helpers;
using InvesTime.BackEnd.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvesTime.BackEnd.Controllers;

[ApiController]
[Route("api/user-statistics")]
[Authorize]
public class UserStatisticsController : ControllerBase
{
    private readonly IUserStatisticsService _userStatisticsService;
    private readonly IMeetingService _meetingService;

    public UserStatisticsController(IUserStatisticsService userStatisticsService, IMeetingService meetingService)
    {
        _userStatisticsService = userStatisticsService;
        _meetingService = meetingService;
    }

    [HttpPost("target", Name = "SetTargetNrOfClientsPerYear")]
    public IActionResult SetTargetNrOfClientsPerYear(int targetNrOfClientsPerYear)
    {
        try
        {
            _userStatisticsService.SetTargetNrOfClientsPerYear(targetNrOfClientsPerYear);
            return Ok($"Clients target per year = {targetNrOfClientsPerYear}");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("target", Name = "GetTargetNrOfClientsPerYear")]
    public IActionResult GetTargetNrOfClientsPerYear()
    {
        try
        {
            var targetNrOfClientsPerYear = _userStatisticsService.GetUserStatistics();
            return Ok(targetNrOfClientsPerYear);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("meetings", Name = "GetMeetingsOfCurrentUser")]
    public IActionResult GetMeetingsOfCurrentUser()
    {
        try
        {
            var meetings = _meetingService.GetMeetingsOfCurrentUser();
            return Ok(meetings);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("consultant/all", Name = "GetUserStatisticsForAdmin")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetUserStatisticsForAdmin()
    {
        try
        {
            var userStatistics = _userStatisticsService.GetAllUserStatistics();
            return Ok(userStatistics);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("consultant/{consultantUsername}/meetings", Name = "GetMeetingsOfMeetingTypeByConsultantUsername")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetMeetingsOfMeetingTypeByConsultantUsername(string consultantUsername, string meetingType,  string startDay, string endDay)
    {
        var startDate = DateConversionHelper.ConvertToDateTime(startDay, "00:00");
        var endDate = DateConversionHelper.ConvertToDateTime(endDay, "00:00");
        try
        {
            var meetings = _meetingService.GetMeetingsOfMeetingTypeByConsultantUsername(consultantUsername, meetingType, startDate, endDate);
            return Ok(meetings);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
