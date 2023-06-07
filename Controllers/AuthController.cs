﻿using backend.Models;
using backend.Models.DTO;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IAuthService _authService;
    private readonly IRegistrationRequestService _registrationRequestService;

    public AuthController(IConfiguration configuration, IAuthService authService, IRegistrationRequestService registrationRequestService)
    {
        _configuration = configuration;
        _authService = authService;
        _registrationRequestService = registrationRequestService;
    }


    // [HttpPost("insertAdmin", Name = "InsertAdmin")] // testing purposes, delete afterwards
    // public ActionResult<User> AskForAccount(RegistrationRequest registrationRequest)
    // {
    //     return _authService.CreateUserWithDefaultPassword(registrationRequest); // make admin from database
    // }


    [HttpPost("askForAccount", Name = "AskForAccount")]
    public ActionResult<RegistrationRequest> AskForAccount(RegistrationRequestDto request)
    {
        var manager = _authService.GetUserByUserName(request.ManagerUsername);
        if (manager is null)
        {
            return BadRequest("The Manager username provided does not exist in our database.");
        }
        else if (manager.IsAdmin == false)
        {
            return BadRequest($"{request.ManagerUsername} cannot approve your request because of insufficient rights. ");
        }

        return Ok(_registrationRequestService.AddRegistrationRequest(request));
    }


    [HttpGet("registration-requests/{managerUsername}", Name = "GetRegistrationRequestsByManager"), Authorize(Roles = "Admin")]
    public ActionResult<IList<RegistrationRequest>> GetRegistrationRequestsByManager(string managerUsername)
    {
        var requests = _registrationRequestService.GetRegistrationRequestsByManagerName(managerUsername);

        return Ok(requests);
    }


    [HttpDelete("registration-requests/{requestId}/delete", Name = "RejectRegistrationRequest"), Authorize(Roles = "Admin")]
    public ActionResult<bool> RejectRegistrationRequest(string requestId)
    {
        var result = _registrationRequestService.DeleteRegistrationRequest(requestId);

        return Ok(result);
    }


    [HttpPost("registration-requests/{requestId}/approve", Name = "ApproveRegistrationRequest"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<User>> ApproveRegistrationRequest(string requestId)
    {
        var registrationRequest = _registrationRequestService.GetRegistrationRequest(requestId);
        var user = _authService.CreateUserWithDefaultPassword(registrationRequest);

        if (await _registrationRequestService.ApproveRegistrationRequest(requestId) != true)
        {
            return BadRequest("Failed to approve registration request.");
        }

        return Ok(user);
    }


    [HttpPost("login", Name = "Login")]
    public ActionResult<string> Login(UserDto request)
    {
        var user = _authService.GetUserByUserName(request.Username);

        if (user == null)
            return BadRequest("The user was not found");

        if (!_authService.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            return BadRequest("Wrong password!");

        var token = _authService.CreateToken(user);

        return Ok(token);
    }

    //to be modified in  GetAllConsultants
    [HttpGet("/users", Name = "GetAllUsers")] //testing purposes rn
    public ActionResult<IList<User>> GetAllUsers()
    {
        return Ok(_authService.GetAllUsers());
    }
}