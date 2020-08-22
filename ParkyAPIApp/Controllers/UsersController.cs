using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkyAPIApp.Models;
using ParkyAPIApp.Repository.IRepository;

namespace ParkyAPIApp.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepo;

        public UsersController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticationModel model)
        {
            var user = _userRepo.Authenticate(model.Username, model.Password);
            if (user == null)
            {
                return BadRequest(new {message = "Username or password is incorrect"});
            }

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] AuthenticationModel model)
        {
            if (!_userRepo.IsUniqueUser(model.Username))
            {
                return BadRequest(new {message = "User already exists"});
            }

            var user = _userRepo.Register(model.Username, model.Password);

            if (user == null)
            {
                return BadRequest(new {message = "Error while registering"});
            }

            return Ok();
        }
    }
}
