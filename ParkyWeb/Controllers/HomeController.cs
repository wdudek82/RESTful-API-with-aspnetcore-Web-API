using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModel;
using ParkyWeb.Repository.IRepository;

namespace ParkyWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly INationalParkRepository _npRepo;
        private readonly ITrailRepository _trailRepo;
        private readonly IAccountRepository _accountRepo;

        public HomeController(INationalParkRepository npRepo, ITrailRepository trailRepo,
            IAccountRepository accountRepo)
        {
            _npRepo = npRepo;
            _trailRepo = trailRepo;
            _accountRepo = accountRepo;
        }

        public async Task<IActionResult> Index()
        {
            var nationalParksAndTrails = new IndexVm
            {
                NationalParks = await _npRepo.GetAllAsync(StaticDetails.NationalParksApiPath),
                Trails = await _trailRepo.GetAllAsync(StaticDetails.TrailsApiPath)
            };
            return View(nationalParksAndTrails);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        [HttpGet]
        public IActionResult Login()
        {
            var obj = new User();
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User obj)
        {
            var user = await _accountRepo.LoginAsync(StaticDetails.LoginApiPath, obj);
            if (user.Token == null)
            {
                return View();
            }

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Username));
            identity.AddClaim(new Claim(ClaimTypes.Role, user.Role));
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            HttpContext.Session.SetString("JWToken", user.Token);

            // Just testing TempData
            TempData["alert"] = $"Welcome {user.Username}";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User obj)
        {
            var result = await _accountRepo.RegisterAsync(StaticDetails.RegisterApiPath, obj);
            if (!result)
            {
                return View();
            }

            // Just testing TempData
            TempData["alert"] = "Registration Successful!";

            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString("JWToken", "");
            return RedirectToAction(nameof(Index));
        }
    }
}
