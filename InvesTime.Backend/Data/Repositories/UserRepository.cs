using InvesTime.BackEnd.Models;
using MongoDB.Driver;

namespace InvesTime.BackEnd.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _users;

    public UserRepository(IMongoDatabase database)
    {
        _users = database.GetCollection<User>("users");
    }

    public IList<User> GetAllUsers()
    {
        return _users.Find(_ => true).ToList();
    }

    public IList<User> GetAllConsultantUsernamesUnderManager(string managerUsername)
    {
        return _users.Find(u => u.ManagerUsername == managerUsername && !u.IsAdmin).ToList();
    }

    public User? GetUserByUsername(string userName)
    {
        return _users.Find(user => user.Username == userName).FirstOrDefault();
    }

    public User? GetUserById(string id)
    {
        return _users.Find(user => user.Id == id).FirstOrDefault();
    }

    public bool ExistsByUsername(string username)
    {
        return _users.AsQueryable().Any(user => user.Username == username);
    }

    public User CreateUser(User user)
    {
        _users.InsertOne(user);
        return user;
    }

    public void UpdateUser(User user)
    {
        _users.ReplaceOne(u => u.Id == user.Id, user);
    }

    public bool DeleteUser(string id)
    {
        var result = _users.DeleteOne(user => user.Id == id);
        return result.DeletedCount > 0;
    }
}