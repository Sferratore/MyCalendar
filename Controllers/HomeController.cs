using Microsoft.AspNetCore.Mvc;
using MyCalendar.Data;
using MyCalendar.Models;
using System.Diagnostics;

namespace MyCalendar.Controllers
{
    //HomeController handles all the operations done in the public part of the web application.
    //The public part is the set of sections that are accessible without being logged in.
    //.NET sets the default name of the controller as its name without the "Controller" part.
    //So, default path for this controller is /Home.
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;  //DInjected in constructor. Used to log.
        private readonly ApplicationDbContext _db; //DInjected in constructor. Main DbContext.

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger; 
            this._db = db;
        }

        //Returns Index.cshtml view when /Index is called with a GET action. Default action for Home.
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        //Returns Error.cshtml view when /Error is called with a GET action.
        //Passes an ErrorViewModel object to display data on view.
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)] //Disables caching of the response to this call.
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //Returns Register.cshtml view when /Register is called with a GET action.
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        //Returns Login.cshtml view when /Login is called with a GET action.
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        //Registers user into DB when /Register is called with a POST action.
        //It can be called by Register.cshtml view. Uses values inserted in view's form to perform registration of the new record.
        [HttpPost]
        public IActionResult Register(UserViewModel formUser)
        {
            if (ModelState.IsValid) //Checks if all the values inserted in the form are correct for an User object.
            {
                //Taking data from UserViewModel and creating an User object to insert into DB.
                User toAdd = new User();
                toAdd.Email = formUser.Email;
                toAdd.Password = formUser.Password;
                toAdd.Username = formUser.Username;
                //Actually adding User to DB.
                _db.Users.Add(toAdd);
                _db.SaveChanges();
                //Getting back to Index view with feedback info.
                TempData["operationFeedback"] = "Your account has been created successfully."; //Displays operation feedback on next called view.
                return RedirectToAction("Index");
            }
            return View(formUser); //If UserViewModel object has non-valid values, gets back at the Register.cshtml view with the values previously inserted. It will display errors because recalling with the model is a .NET MVC convention and does it all by default.
        }

        //Logs user into DB when /Login is called with a POST action.
        //It can be called by Login.cshtml view. Uses values inserted in view's form to perform login of user.
        [HttpPost]
        public IActionResult Login(UserViewModel formUser)
        {
            User? loggedInUser = _db.Users.FirstOrDefault<User>(u => u.Username == formUser.Username && u.Password == formUser.Password); //Retrieving user from DB.

            if (loggedInUser != null) //If user has been found...
            {
                //Setting in session the credentials of user.
                HttpContext.Session.SetInt32("loggedInId", loggedInUser.IdUser); 
                HttpContext.Session.SetString("loggedInUsername", loggedInUser.Username);
                //Redirect user to his UserSection's Index
                return RedirectToRoute(new
                {
                    controller = "UserSection",
                    action = "Index",
                });
            }

            //If login fails, gets user back to Login view and displays error message.
            TempData["loginOperationFeedback"] = "Your login credentials are wrong.";
            return View(formUser);
        }
    }
}