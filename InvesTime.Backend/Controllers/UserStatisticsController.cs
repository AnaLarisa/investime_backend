using InvesTime.BackEnd.Services;
using InvesTime.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace InvesTime.BackEnd.Controllers;

[ApiController]
[Route("statistics")]
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
    [HttpPost("targetNrOfClientsPerYear", Name = "SetTargetNrOfClientsPerYear")]
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
    /// Get personal full statistics between start and end date
    /// </summary>
    [HttpGet("fullStatistics", Name = "GetFullPersonalStatistics")]
    public ActionResult<UserStatisticsDateRangeDto> GetFullStatisticsDateRange(
        [FromQuery(Name = "startDate")][Required][SwaggerParameter("yyyy-MM-dd")] DateTime startDate,
        [FromQuery(Name = "endDate")][Required][SwaggerParameter("yyyy-MM-dd")] DateTime endDate)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (startDate >= endDate)
        {
            return BadRequest("Start date must be before end date.");
        }

        try
        {
            var userStatisticsDateRange = _userStatisticsService.GetUserStatisticsDateRangeDto(startDate, endDate);
            return userStatisticsDateRange;
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }


    /// <summary>
    /// Get a consultant's full statistics between start and end date by username
    /// </summary>
    [HttpGet("fullStatistics/{username}", Name = "GetFullUserStatisticsForUsername")]
    public IActionResult GetFullUserStatisticsForUsername(string username,
        [FromQuery(Name = "startDate")][Required][SwaggerParameter("yyyy-MM-dd")] DateTime startDate,
        [FromQuery(Name = "endDate")][Required][SwaggerParameter("yyyy-MM-dd")] DateTime endDate)
    {
        try
        {
            var userStatistics = _userStatisticsService.GetUserStatisticsDateRangeDto(startDate, endDate, username);
            return Ok(userStatistics);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    /// <summary>
    /// Get personal list of goals
    /// </summary>
    [HttpGet("goals/all", Name = "GetPersonalGoalsList")]
    public ActionResult<IList<string>> GetPersonalGoalsList()
    {
        try
        {
            var targetLists = _userStatisticsService.GetGoalsListsForCurrentUser();
            if (targetLists.Count > 0)
            {
                return Ok(targetLists);
            }
            else
            {
                return NotFound("Your goal list currently has no entries. Begin by adding your first goal!");
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    /// <summary>
    /// Add a new goal to personal list of goals
    /// </summary>
    /// <param name="goal"></param>
    [HttpPost("goals/add", Name = "AddGoalToList")]
    public IActionResult AddGoalToList(string goal)
    {
        try
        {
            _userStatisticsService.AddGoalToList(goal);
            return Ok($"Goal \"{goal}\" added to list");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    /// <summary>
    /// Delete a goal from personal list of goals
    /// </summary>
    /// <param name="goal"></param>
    [HttpDelete("goals/delete", Name = "RemoveGoalToList")]
    public IActionResult RemoveGoalToList(string goal)
    {
        try
        {
            _userStatisticsService.RemoveGoalFromList(goal);
            return Ok($"Goal \"{goal}\" removed from list");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}