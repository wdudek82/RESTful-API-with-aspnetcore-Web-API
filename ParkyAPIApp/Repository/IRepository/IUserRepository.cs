using System.Collections.Generic;
using ParkyAPIApp.Models;

namespace ParkyAPIApp.Repository.IRepository
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsers();

        User GetUserById(int userId);

        bool UserExists(string username);

        bool CreateUser(User user);

        bool UpdateUser(User user);

        bool DeleteUser(User user);

        bool Save();
    }
}
