using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ParkyAPIApp.Data;
using ParkyAPIApp.Models;

namespace ParkyAPIApp.Repository.IRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly AppSettings _appSettings;

        public UserRepository(ApplicationDbContext db, IOptions<AppSettings> appSettings)
        {
            _db = db;
            _appSettings = appSettings.Value;
        }

        public bool IsUniqueUser(string username)
        {
            return _db.Users.AsEnumerable().Any(u => Normalize(u.Username) == Normalize(username));
        }

        public User Authenticate(string username, string password)
        {
            var user = _db.Users.SingleOrDefault(u => u.Username == username && u.Password == password);

            if (user == null)
            {
                return null;
            }

            // if user was found generate JWT Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            return user;
        }

        public User Register(string username, string password)
        {


            var user = new User
            {
                Username = username,
                Password = password,
                // Role = ""
            };

            _db.Users.Add(user);
            _db.SaveChanges();

            user.Password = "";

            return user;
        }

        private static string Normalize(string text)
        {
            return text.ToLower().Trim();
        }
    }
}
