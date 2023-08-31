using InvesTime.BackEnd.Models;
using InvesTime.BackEnd.Models.DTO;
using InvesTime.BackEnd.Services;
using InvesTime.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvesTime.BackEnd.Controllers;

[Route("auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IRegistrationRequestService _registrationRequestService;
    private readonly IUserService _userService;

    public AuthController(IConfiguration configuration, IUserService userService,
        IRegistrationRequestService registrationRequestService)
    {
        _configuration = configuration;
        _userService = userService;
        _registrationRequestService = registrationRequestService;
    }


    /// <summary>
    /// Ask for an new consultant account.
    /// </summary>
    /// <param name="request"></param>
    [HttpPost("askForAccount", Name = "AskForAccount")]
    public ActionResult<RegistrationRequest> AskForAccount(RegistrationRequestDto request)
    {
        var manager = _userService.GetUserByUsername(request.ManagerUsername);
        if (manager is null)
            return BadRequest("The Manager username provided does not exist in our database.");
        if (manager.IsAdmin == false)
            return BadRequest(
                $"{request.ManagerUsername} cannot approve your request because of insufficient rights. ");

        return Ok(_registrationRequestService.AddRegistrationRequest(request));
    }


    /// <summary>
    /// Get all the registration requests the logged in manager has on his name.
    /// </summary>
    [HttpGet("registration-requests", Name = "GetRegistrationRequestsByManager")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IList<RegistrationRequest>>> GetRegistrationRequestsByManager()
    {
        var requests = await _registrationRequestService.GetRegistrationRequestsByManagerName();
        if (requests.Count > 0)
        {
            return Ok(requests);
        }

        return NotFound("No registration requests found.");
    }


    /// <summary>
    /// Approve one registration request.
    /// </summary>
    /// <param name="requestId"></param>
    [HttpPost("registration-requests/{requestId}/approve", Name = "ApproveRegistrationRequest")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<User>> ApproveRegistrationRequest(string requestId)
    {
        var registrationRequest = _registrationRequestService.GetRegistrationRequest(requestId);

        if (registrationRequest == null) return BadRequest("The registration request was not found in the database.");
        var user = _userService.CreateUserWithDefaultPassword(registrationRequest);

        if (await _registrationRequestService.ApproveRegistrationRequest(requestId) != true)
            return BadRequest("Failed to approve registration request.");

        return Ok(user);
    }


    /// <summary>
    /// Reject one registration request.
    /// </summary>
    /// <param name="requestId"></param>
    [HttpDelete("registration-requests/{requestId}/delete", Name = "RejectRegistrationRequest")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<bool>> RejectRegistrationRequest(string requestId)
    {
        var result = await _registrationRequestService.DeleteRegistrationRequest(requestId);
        if (result)
        {
            return Ok("The registration request has been successfully deleted.");
        }

        return BadRequest("The registration request was not found in the database.");
    }


    /// <summary>
    /// Login with username and password.
    /// </summary>
    /// <param name="request"></param>
    [HttpPost("login", Name = "Login")]
    public ActionResult<UserInfoDto> Login(UserDto request)
    {
        var user = _userService.GetUserByUsername(request.Username);
        if (user == null)
            return BadRequest("The user was not found");

        if (!_userService.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            return BadRequest("Wrong password!");

        var token = _userService.CreateToken(user);

        return _userService.GetUserInformation(user, token);
    }
}