using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using InvesTime.BackEnd.Data.Repositories;
using InvesTime.BackEnd.Helpers;
using InvesTime.BackEnd.Models;
using InvesTime.BackEnd.Models.DTO;
using Microsoft.IdentityModel.Tokens;

namespace InvesTime.BackEnd.Services;

public class UserService : IUserService
{
    private readonly IConfiguration _configuration;
    private readonly IMeetingRepository _meetingRepository;
    private readonly IUserHelper _userHelper;
    private readonly IUserRepository _userRepository;

    public UserService(IConfiguration configuration, IUserHelper userHelper, IUserRepository userRepository,
        IMeetingRepository meetingRepository)
    {
        _configuration = configuration;
        _userHelper = userHelper;
        _userRepository = userRepository;
        _meetingRepository = meetingRepository;
    }


    public User? GetUserByUsername(string userName)
    {
        return _userRepository.GetUserByUsername(userName);
    }

    public string GetUserIdByUsername(string userName)
    {
        var user = _userRepository.GetUserByUsername(userName);
        if (user == null) throw new InvalidDataException("The current user is not present in the database.");
        return user.Id;
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
            Email = registrationRequest.Email
        };

        return _userRepository.CreateUser(user);
    }


    public string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.NameIdentifier, user.Id),
            user.IsAdmin
                ? new Claim(ClaimTypes.Role, "Admin")
                : new Claim(ClaimTypes.Role, "User")
        };


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


    public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return StructuralComparisons.StructuralEqualityComparer.Equals(computedHash, passwordHash);
    }


    public bool ChangePassword(ChangePasswordDto changePasswordDto)
    {
        var user = _userRepository.GetUserById(_userHelper.GetCurrentUserId());
        if (user == null) throw new InvalidDataException("The current user is not present in the database.");

        var isPasswordCorrect =
            VerifyPasswordHash(changePasswordDto.CurrentPassword, user.PasswordHash, user.PasswordSalt);
        if (!isPasswordCorrect) throw new InvalidDataException("Incorrect current password.");

        CreatePasswordHash(changePasswordDto.NewPassword, out var newPasswordHash, out var newPasswordSalt);
        user.PasswordHash = newPasswordHash;
        user.PasswordSalt = newPasswordSalt;
        _userRepository.UpdateUser(user);

        return true;
    }

    public bool DeleteConsultant(string username)
    {
        var user = _userRepository.GetUserByUsername(username);
        if (user == null) throw new InvalidDataException("The current user is not present in the database.");
        if (user.IsAdmin) throw new InvalidDataException("The user is an admin and cannot be deleted.");

        DeleteAllMeetingsOfUserId(user.Id);

        var deleteUser = _userRepository.DeleteUser(user.Id);
        if (!deleteUser) throw new InvalidDataException("The delete operation failed.");

        return true;
    }


    public void DeleteAllMeetingsOfUserId(string userId)
    {
        var deletedMeetings = _meetingRepository.DeleteAllMeetingsOfUserId(userId);
        if (deletedMeetings == false) throw new InvalidDataException("The user's meetings cannot be deleted.");
    }


    public bool IsUsernameTaken(string username)
    {
        return _userRepository.ExistsByUsername(username);
    }

    public IList<string> GetAllConsultantUsernamesUnderManager()
    {
        var managerUsername = _userHelper.GetCurrentUserUsername();
        return _userRepository.GetAllConsultantUsernamesUnderManager(managerUsername).Select(u => u.Username).ToList();
    }


    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }
}