using backend.Models;
using backend.Models.DTO;

namespace backend.Services;

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