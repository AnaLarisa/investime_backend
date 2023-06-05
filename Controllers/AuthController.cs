using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IAuthService _authService;

    public AuthController(IConfiguration configuration, IAuthService authService)
    {
        _configuration = configuration;
        _authService = authService;
    }

    [HttpPost("register", Name = "Register")]
    public ActionResult<User> Register(UserDto request)
    { 
        return Ok(_authService.CreateUser(request));
    }

    [HttpPost("login", Name = "Login")]
    public ActionResult<string> Login(UserDto request)
    {
        var user = _authService.GetUserByUserName(request.UserName);

        if (user == null)
            return BadRequest("The user was not found");

        if (!_authService.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            return BadRequest("Wrong password!");

        var token = _authService.CreateToken(user);

        return Ok(token);
    }

    //to be modified in  GetAllConsultants
    [HttpGet("/users", Name = "GetAllUsers")]
    public ActionResult<IList<User>> GetAllUsers()
    {
        return Ok(_authService.GetAllUsers());
    }
}