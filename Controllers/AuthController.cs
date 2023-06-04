using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    public static User user = new User();
    private readonly IConfiguration _configuration;
    private readonly AuthService _authService;

    public AuthController(IConfiguration configuration, AuthService authService)
    {
        _configuration = configuration;
        _authService = authService;
    }

    [HttpPost("register", Name = "Register")]
    public ActionResult<User> Register(UserDto request)
    {
        _authService.CreatePasswordHash(request.Password, out var passwordHash, out var passwordSalt);

        user.UserName = request.UserName;
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        return Ok(user);
    }

    [HttpPost("login", Name = "Login")]
    public ActionResult<string> Login(UserDto request)
    {
        if (user.UserName != request.UserName)
            return BadRequest("The user was not found");

        if (!_authService.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            return BadRequest("Wrong password!");

        var token = _authService.CreateToken(user);

        return Ok(token);
    }
}