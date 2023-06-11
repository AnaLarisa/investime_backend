using InvesTime.BackEnd.Models.DTO;
using InvesTime.BackEnd.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvesTime.BackEnd.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("/my/consultants/", Name = "GetAllConsultantUsernamesUnderManager")]
    [Authorize(Roles = "Admin")]
    public ActionResult<IList<string>> GetAllConsultantUsernamesUnderManager()
    {
        return Ok(_userService.GetAllConsultantUsernamesUnderManager());
    }

    [HttpPut("changePassword", Name = "ChangePassword")]
    [Authorize]
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


    [HttpDelete("{consultantUsername}", Name = "DeleteConsultant")]
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