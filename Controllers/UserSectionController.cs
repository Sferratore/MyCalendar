using Microsoft.AspNetCore.Mvc;
using MyCalendar.Data;
using MyCalendar.Models;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyCalendar.Controllers
{
    // UserSectionController manages user-specific actions and views in the web application.
    // This controller handles the user's personal section, which includes calendar management.
    public class UserSectionController : Controller
    {
        private readonly ApplicationDbContext _db; // DInjected in constructor. Main DbContext.

        public UserSectionController(ApplicationDbContext db, IHttpClientFactory httpClientFactory)
        {
            this._db = db;
        }

        // Default action for UserSection. Returns Index view of UserSection.
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

    }
}
