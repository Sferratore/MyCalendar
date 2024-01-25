using Microsoft.AspNetCore.Mvc;

namespace MyCalendar.Controllers
{
    public class TodayWeatherController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
