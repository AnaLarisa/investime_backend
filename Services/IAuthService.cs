using backend.Models;

namespace backend.Services;

public interface IAuthService
{
    public string? GetCurrentUserId();
    public User? GetUserByUserName(string userName);
    public User CreateUserWithDefaultPassword(RegistrationRequest registrationRequest);
    public string CreateToken(User user);
    public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    public bool IsUsernameTaken(string username);
    public IList<User> GetAllUsers();
}