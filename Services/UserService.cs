using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using InvesTime.BackEnd.Data.Repositories;
using InvesTime.BackEnd.Models;
using InvesTime.BackEnd.Models.DTO;
using Microsoft.IdentityModel.Tokens;

namespace InvesTime.BackEnd.Services;

public class UserService : IUserService
{
    private readonly IConfiguration _configuration;
    private IHttpContextAccessor _contextAccessor;
    private IUserRepository _userRepository;
    private IMeetingRepository _meetingRepository;

    public UserService(IConfiguration configuration, IHttpContextAccessor contextAccessor, IUserRepository userRepository, IMeetingRepository meetingRepository)
    {
        _configuration = configuration;
        _contextAccessor = contextAccessor;
        _userRepository = userRepository;
        _meetingRepository = meetingRepository;
    }


    public string? GetCurrentUserId()
    {
        var httpContext = _contextAccessor.HttpContext;
        var claimsIdentity = httpContext?.User.Identity as ClaimsIdentity;
        return claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
    
    private string? GetCurrentUserUsername()
    {
        var httpContext = _contextAccessor.HttpContext;
        var claimsIdentity = httpContext?.User.Identity as ClaimsIdentity;
        return claimsIdentity?.FindFirst(ClaimTypes.Name)?.Value;
    }


    public User? GetUserByUsername(string userName)
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


    public bool ChangePassword(ChangePasswordDto changePasswordDto)
    {
        var user = _userRepository.GetUserById(GetCurrentUserId()!);
        if (user == null)
        {
            throw new InvalidDataException("The current user is not present in the database.");
        }

        var isPasswordCorrect = VerifyPasswordHash(changePasswordDto.CurrentPassword, user.PasswordHash, user.PasswordSalt);
        if (!isPasswordCorrect)
        {
            throw new InvalidDataException("Incorrect current password.");
        }

        CreatePasswordHash(changePasswordDto.NewPassword, out var newPasswordHash, out var newPasswordSalt);
        user.PasswordHash = newPasswordHash;
        user.PasswordSalt = newPasswordSalt;
        _userRepository.UpdateUser(user);

        return true;
    }

    public bool DeleteConsultant(string username)
    {
        var user = _userRepository.GetUserByUsername(username);
        if (user == null)
        {
            throw new InvalidDataException("The current user is not present in the database.");
        }
        else if (user.IsAdmin == true)
        {
            throw new InvalidDataException("The user is an admin and cannot be deleted.");
        }

        var deletedMeetings = _meetingRepository.DeleteAllMeetingsOfUserId(user.Id);
        if (deletedMeetings == false)
        {
            throw new InvalidDataException("The user's meetings cannot be deleted.");
        }


        var deleteUser = _userRepository.DeleteUser(user.Id);
        if (!deleteUser)
        {
            throw new InvalidDataException("The delete operation failed.");
        }

        return true;
    }


    public bool IsUsernameTaken(string username)
    { 
        return _userRepository.ExistsByUsername(username);
    }

    public IList<string> GetAllConsultantUsernamesUnderManager()
    {
        var managerUsername = GetCurrentUserUsername();
        return _userRepository.GetAllConsultantUsernamesUnderManager(managerUsername!).Select(u => u.Username).ToList();
    }
}
