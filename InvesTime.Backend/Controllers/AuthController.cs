using InvesTime.BackEnd.Models;
using InvesTime.BackEnd.Models.DTO;
using InvesTime.BackEnd.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvesTime.BackEnd.Controllers;

[Route("api/[controller]")]
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


    [HttpGet("registration-requests", Name = "GetRegistrationRequestsByManager")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IList<RegistrationRequest>>> GetRegistrationRequestsByManager()
    {
        var requests = await _registrationRequestService.GetRegistrationRequestsByManagerName();

        return Ok(requests);
    }


    [HttpDelete("registration-requests/{requestId}/delete", Name = "RejectRegistrationRequest")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<bool>> RejectRegistrationRequest(string requestId)
    {
        var result = await _registrationRequestService.DeleteRegistrationRequest(requestId);
        if (result == false) return BadRequest("The registration request was not found in the database.");

        return Ok("The registration request has been successfully deleted.");
    }


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


    [HttpPost("login", Name = "Login")]
    public ActionResult<string> Login(UserDto request)
    {
        var user = _userService.GetUserByUsername(request.Username);
        if (user == null)
            return BadRequest("The user was not found");

        if (!_userService.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            return BadRequest("Wrong password!");

        var token = _userService.CreateToken(user);

        return Ok(token);
    }
}