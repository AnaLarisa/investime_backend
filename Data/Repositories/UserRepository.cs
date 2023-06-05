using backend.Models;
using MongoDB.Driver;

namespace backend.Data.Repositories
{
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

        public User GetUserByUsername(string userName)
        {
            return _users.Find(user => user.UserName == userName).FirstOrDefault();
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

        public void DeleteUser(string id)
        {
            _users.DeleteOne(user => user.Id == id);
        }
    }
}