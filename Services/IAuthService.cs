using backend.Models;

namespace backend.Services;

public interface IAuthService
{
    public string? GetCurrentUserId();
    public User? GetUserByUserName(string userName);
    public User CreateUser(UserDto userDto);
    public string CreateToken(User user);
    public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    public IList<User> GetAllUsers();
}