using InvesTime.BackEnd.Models;
using InvesTime.BackEnd.Models.DTO;

namespace InvesTime.BackEnd.Services;

public interface IUserService
{
    public string? GetCurrentUserId(); 
    public User? GetUserByUsername(string userName); 
    public User CreateUserWithDefaultPassword(RegistrationRequest registrationRequest);
    public string CreateToken(User user);
    public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    public bool ChangePassword(ChangePasswordDto changePasswordDto);
    public bool DeleteConsultant(string username);
    public bool IsUsernameTaken(string username);
    public IList<string> GetAllConsultantUsernamesUnderManager();
}