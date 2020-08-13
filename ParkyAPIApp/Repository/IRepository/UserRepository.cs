using System.Collections.Generic;
using System.Linq;
using ParkyAPIApp.Data;
using ParkyAPIApp.Models;

namespace ParkyAPIApp.Repository.IRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public IEnumerable<User> GetUsers()
        {
            return _db.Users.ToList();
        }

        public User GetUserById(int userId)
        {
            return _db.Users.FirstOrDefault(u => u.Id == userId);
        }

        public bool UserExists(string username)
        {
            return _db.Users.Any(u => Normalize(u.Username) == Normalize(username));
        }

        private static string Normalize(string text)
        {
            return text.ToLower().Trim();
        }

        public bool CreateUser(User user)
        {
            _db.Users.Add(user);
            return Save();
        }

        public bool UpdateUser(User user)
        {
            _db.Users.Update(user);
            return Save();
        }

        public bool DeleteUser(User user)
        {
            _db.Users.Remove(user);
            return Save();
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0;
        }
    }
}
