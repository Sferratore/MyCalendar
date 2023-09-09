using Microsoft.AspNetCore.Mvc;
using MyCalendar.Data;
using MyCalendar.Models;
using System.Diagnostics;

namespace MyCalendar.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            this._db = db;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User formUser)
        {
            if (ModelState.IsValid)
            {
                _db.Users.Add(formUser);
                _db.SaveChanges();
                TempData["successForAccount"] = "Your account has been created successfully.";
                return RedirectToAction("Index");
            }
            return View(formUser);
        }

        [HttpPost]
        public IActionResult Login(User formUser)
        {
            User loggedInUser = _db.Users.FirstOrDefault<User>(u => u.Username == formUser.Username && u.Password == formUser.Password);

            if (loggedInUser != null)
            {
                HttpContext.Session.SetInt32("loggedInId", loggedInUser.IdUser);
                HttpContext.Session.SetString("loggedInUsername", loggedInUser.Username);
                return RedirectToRoute(new
                {
                    controller = "UserSection",
                    action = "Index",
                });
            }

            return RedirectToAction("Index");
        }
    }
}