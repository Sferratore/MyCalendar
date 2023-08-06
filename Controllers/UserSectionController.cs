using Microsoft.AspNetCore.Mvc;
using MyCalendar.Data;
using MyCalendar.Models;

namespace MyCalendar.Controllers
{
    public class UserSectionController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UserSectionController(ApplicationDbContext db)
        {
            this._db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Calendar()
        {
            IEnumerable<CalendarAnnotation> objCalendarAnnotationsList = _db.CalendarAnnotations.Where(a => a.User.IdUser == HttpContext.Session.GetInt32("loggedInId")).OrderBy(a => a.Date);
            return View(objCalendarAnnotationsList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CalendarAnnotation formAnnotation)
        {
            if (ModelState.ErrorCount == 1) //Checks for a single error to work around the invalid user field of model CalendarAnnotation
            {
                formAnnotation.User = _db.Users.First(u => u.IdUser == HttpContext.Session.GetInt32("loggedInId"));
                _db.CalendarAnnotations.Add(formAnnotation);
                _db.SaveChanges();
                TempData["successForAnnotation"] = "Your annotation has been created successfully.";
                return RedirectToAction("Calendar");
            }
            return View(formAnnotation);
        }

        public IActionResult Edit(int idCalendar)
        {
            CalendarAnnotation toEdit = _db.CalendarAnnotations.First(a => a.IdCalendar == idCalendar);
            return View(toEdit);
        }

        [HttpPost]
        public IActionResult Edit(CalendarAnnotation formAnnotation)
        {
            if (ModelState.ErrorCount == 1) //Checks for a single error to work around the invalid user field of model CalendarAnnotation
            {
                _db.CalendarAnnotations.Update(formAnnotation);
                _db.SaveChanges();
                TempData["successForAnnotation"] = "Your annotation has been edited successfully.";
                return RedirectToAction("Calendar");
            }
            return View(formAnnotation);
        }

        public IActionResult Delete(int idCalendar)
        {
            CalendarAnnotation obj = _db.CalendarAnnotations.First(a => a.IdCalendar == idCalendar);
            _db.CalendarAnnotations.Remove(obj);
            _db.SaveChanges();
            TempData["successForAnnotation"] = "Your annotation has been deleted successfully.";
            return RedirectToAction("Calendar");
        }
    }
}
