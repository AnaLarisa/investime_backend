using InvesTime.BackEnd.Models;

namespace InvesTime.BackEnd.Data.Repositories;

public interface IUserRepository
{
    public User CreateUser(User user);
    public bool DeleteUser(string id);
    public IList<User> GetAllUsers();
    public IList<User> GetAllConsultantUsernamesUnderManager(string managerUsername);
    public User? GetUserByUsername(string userName);
    public User? GetUserById(string id);
    public IList<User> GetAllManagers();
    public bool ExistsByUsername(string username);
    public void UpdateUser(User user);
}