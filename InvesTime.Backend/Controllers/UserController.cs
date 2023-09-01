using InvesTime.BackEnd.Models.DTO;
using InvesTime.BackEnd.Services;
using InvesTime.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvesTime.BackEnd.Controllers;

[Route("user")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }


    /// <summary>
    /// Updates the user's information. Everything can be sent empty except the manager username.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="userUpdateInfoDto"></param>
    /// <returns></returns>
    [HttpPut("updateUser", Name = "UpdateUserById")]
    public ActionResult UpdateUserById(UserUpdateInfoDto userUpdateInfoDto)
    {
        try
        {
            _userService.UpdateCurrentUser(userUpdateInfoDto);

        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return Ok("User updated successfully.");
    }



    /// <summary>
    /// Admin: Get a list of all the consultants the logged in manager has.
    /// </summary>
    [HttpGet("team", Name = "GetAllConsultantsUnderManager")]
    public ActionResult<IList<string>> GetAllConsultantUsernamesUnderManager()
    {
        var usernameList = _userService.GetAllConsultantsUnderManager();
        if (usernameList.Count > 0)
        {
            return Ok(usernameList.Select(u => u.Username).ToList());
        }
        return NotFound("No consultants found.");
    }


    /// <summary>
    /// Gets all the available managers from the database (their usernames)
    /// </summary>
    /// <returns></returns>
    [HttpGet("managers/all", Name = "GetAllManagers")]
    public ActionResult<IList<string>> GetAllManagers()
    {
        var usernameList = _userService.GetAllManagers();
        if (usernameList.Count > 0)
        {
            return Ok(usernameList.Select(u => u.Username).ToList());
        }
        return NotFound("No managers found.");
    }


    /// <summary>
    /// Change the password of the current user.
    /// </summary>
    /// <param name="changePasswordDto"></param>
    [HttpPut("changePassword", Name = "ChangePassword")]
    public ActionResult ChangePassword(ChangePasswordDto changePasswordDto)
    {
        if (!changePasswordDto.NewPassword.Equals(changePasswordDto.NewPasswordAgain))
            return BadRequest("The new password and its repetition do not match.");

        try
        {
            var isPasswordChanged = _userService.ChangePassword(changePasswordDto);
            return !isPasswordChanged
                ? BadRequest("Failed to change the password.")
                : Ok("Password changed successfully.");
        }
        catch (InvalidDataException e)
        {
            return BadRequest(e.Message);
        }
    }


    /// <summary>
    /// Admin: Delete a consultant from the database (along with its meetings).
    /// </summary>
    /// <param name="consultantUsername"></param>
    [HttpDelete("delete/{consultantUsername}", Name = "DeleteConsultant")]
    [Authorize(Roles = "Admin")]
    public ActionResult DeleteConsultant(string consultantUsername)
    {
        try
        {
            _userService.DeleteConsultant(consultantUsername);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return Ok($"{consultantUsername} has been deleted successfully from the database");
    }
}