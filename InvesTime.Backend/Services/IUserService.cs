using InvesTime.BackEnd.Models;
using InvesTime.BackEnd.Models.DTO;
using InvesTime.Models.DTO;

namespace InvesTime.BackEnd.Services;

public interface IUserService
{
    public User? GetUserByUsername(string userName);
    public User CreateUserWithDefaultPassword(RegistrationRequest registrationRequest);
    public string CreateToken(User user);
    public UserInfoDto GetUserInformation(User user, string token);
    public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    public bool ChangePassword(ChangePasswordDto changePasswordDto);
    public void UpdateCurrentUser(UserUpdateInfoDto userDto);
    public bool DeleteConsultant(string username);
    public bool IsUsernameTaken(string username);
    public User? GetUserById(string id);
    public IList<User> GetAllConsultantsUnderManager();
    public IList<User> GetAllManagers();
}