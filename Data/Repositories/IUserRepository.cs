using backend.Models;

namespace backend.Data.Repositories
{
    public interface IUserRepository
    {
        public User CreateUser(User user);
        public void DeleteUser(string id);
        public IList<User> GetAllUsers();
        public User GetUserByUsername(string userName);
        public bool ExistsByUsername(string username);
        public void UpdateUser(User user);
    }
}