using Microsoft.AspNetCore.Mvc;

namespace MyCalendar.Controllers
{
    public class UserSectionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
