using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using backend.Data.Repositories;
using backend.Models;
using Microsoft.IdentityModel.Tokens;

namespace backend.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private IHttpContextAccessor _contextAccessor;
    private IUserRepository _userRepository;

    public AuthService(IConfiguration configuration, IHttpContextAccessor contextAccessor, IUserRepository userRepository)
    {
        _configuration = configuration;
        _contextAccessor = contextAccessor;
        _userRepository = userRepository;
    }

    public string? GetCurrentUserId()
    {
        var httpContext = _contextAccessor.HttpContext;

        var claimsIdentity = httpContext?.User.Identity as ClaimsIdentity;

        return claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    public User? GetUserByUserName(string userName)
    {
        return _userRepository.GetUserByUsername(userName);
    }


    public User CreateUserWithDefaultPassword(RegistrationRequest registrationRequest)
    {
        CreatePasswordHash("InvesTime&User123*", out var passwordHash, out var passwordSalt);
        var user = new User
        {
            FirstName = registrationRequest.FirstName,
            LastName = registrationRequest.LastName,
            Username = registrationRequest.Username,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            IsAdmin = false,
            ManagerUsername = registrationRequest.ManagerUsername,
            Email = registrationRequest.Email,
        };

        return _userRepository.CreateUser(user);
    }


    public string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.NameIdentifier, user.Id)
        };

        claims.Add(user.IsAdmin 
            ? new Claim(ClaimTypes.Role, "Admin") 
            : new Claim(ClaimTypes.Role, "User"));


        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration.GetSection("Authentication:Token").Value!));

        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: cred
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }


    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }

    public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return StructuralComparisons.StructuralEqualityComparer.Equals(computedHash, passwordHash);
        }
    }

    public IList<User> GetAllUsers()
    { 
        return _userRepository.GetAllUsers() ?? new List<User>();
    }
}
