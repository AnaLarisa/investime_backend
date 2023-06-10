using backend.Models.DTO;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers;

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
        {
            return BadRequest("The new password and its repetition do not match.");
        }

        try
        {
            var isPasswordChanged = _userService.ChangePassword(changePasswordDto);
            return !isPasswordChanged 
                ? StatusCode(500, "Failed to change the password.") 
                : Ok("Password changed successfully.");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

}