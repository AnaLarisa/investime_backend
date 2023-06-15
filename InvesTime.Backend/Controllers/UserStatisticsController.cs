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
    private readonly IMeetingService _meetingService;
    private readonly IUserStatisticsService _userStatisticsService;

    public UserStatisticsController(IUserStatisticsService userStatisticsService, IMeetingService meetingService)
    {
        _userStatisticsService = userStatisticsService;
        _meetingService = meetingService;
    }


    /// <summary>
    /// Sets the personal targeted number of clients/current year
    /// </summary>
    /// <param name="targetNrOfClientsPerYear"></param>
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


    /// <summary>
    /// Get the targeted nr of clients + nr of clients until now for current user.
    /// </summary>
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


    /// <summary>
    /// Get all the meetings of current user sorted by Type (to be continued with date range filter)
    /// </summary>
    [HttpGet("meetingsByType", Name = "GetMeetingsOfUserIdSortedByType")]
    public IActionResult GetMeetingsOfCurrentUserSortedByType()
    {
        try
        {
            var meetings = _meetingService.GetMeetingsOfUserIdSortedByType();
            return Ok(meetings);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    /// <summary>
    /// Admin: Get all consultant's statistics (target + nr of clients until now)
    /// </summary>
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


    /// <summary>
    /// Admin: Get all meetings of a type for a selected consultant username.
    /// </summary>
    /// <param name="consultantUsername"></param>
    /// <param name="meetingType"></param>
    /// <param name="startDay"></param>
    /// <param name="endDay"></param>
    [HttpGet("consultant/{consultantUsername}/meetings", Name = "GetMeetingsOfMeetingTypeByConsultantUsername")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetMeetingsOfMeetingTypeByConsultantUsername(string consultantUsername, string meetingType,
        string startDay, string endDay)
    {
        var startDate = DateConversionHelper.ConvertToDateTime(startDay, "00:00");
        var endDate = DateConversionHelper.ConvertToDateTime(endDay, "00:00");
        try
        {
            var meetings =
                _meetingService.GetMeetingsOfMeetingTypeByConsultantUsername(consultantUsername, meetingType, startDate,
                    endDate);
            return Ok(meetings);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}