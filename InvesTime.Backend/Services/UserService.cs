using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using InvesTime.BackEnd.Data.Repositories;
using InvesTime.BackEnd.Helpers;
using InvesTime.BackEnd.Models;
using InvesTime.BackEnd.Models.DTO;
using InvesTime.Models.DTO;
using Microsoft.IdentityModel.Tokens;

namespace InvesTime.BackEnd.Services;

public class UserService : IUserService
{
    private readonly IConfiguration _configuration;
    private readonly IMeetingService _meetingService;
    private readonly IUserHelper _userHelper;
    private readonly IUserRepository _userRepository;
    private readonly IUserStatisticsRepository _userStatisticsRepository;

    public UserService(IConfiguration configuration, IUserHelper userHelper, IUserRepository userRepository,
        IMeetingService meetingService, IUserStatisticsRepository userStatisticsRepository)
    {
        _configuration = configuration;
        _meetingService = meetingService;
        _userHelper = userHelper;
        _userRepository = userRepository;
        _userStatisticsRepository = userStatisticsRepository;
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

        user = _userRepository.CreateUser(user);
        if (user == null)
        {
            throw new InvalidDataException("The user could not be created.");
        }

        _meetingService.AddUpcomingTeamMeetingsForUser(user);

        return user;
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

        _meetingService.DeleteAllMeetingsOfUserId(user.Id);
        DeleteAllUserStatisticsOfUsername(user.Username);

        var deleteUser = _userRepository.DeleteUser(user.Id);
        if (!deleteUser) throw new InvalidDataException("The delete operation failed.");

        return true;
    }

    private void DeleteAllUserStatisticsOfUsername(string username)
    {
        _userStatisticsRepository.DeleteUserStatistics(username);
    }


    public bool IsUsernameTaken(string username)
    {
        return _userRepository.ExistsByUsername(username);
    }

    public IList<User> GetAllConsultantsUnderManager()
    {
        if (_userHelper.IsCurrentUserAdmin())
        {
            var managerUsername = _userHelper.GetCurrentUserUsername();
            return _userRepository.GetAllConsultantUsernamesUnderManager(managerUsername);
        }
        else
        {
            var currentUser = _userRepository.GetUserById(_userHelper.GetCurrentUserId());
            return _userRepository.GetAllConsultantUsernamesUnderManager(currentUser!.ManagerUsername);
        }
    }


    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    public UserInfoDto GetUserInformation(User user, string token)
    {
        return new UserInfoDto
        {
            Id = user.Id,
            AuthorizationToken = token,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Username = user.Username,
            IsAdmin = user.IsAdmin,
            ManagerUsername = user.ManagerUsername,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Address = user.Address,
            City = user.City,
            EmailConfirmed = user.EmailConfirmed,
            MeetingsNotificationsOff = user.MeetingsNotificationsOff,
        };
    }
}