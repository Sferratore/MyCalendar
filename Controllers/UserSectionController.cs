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
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TaskReminder formTask)
        {
            if (ModelState.IsValid)
            {
                _db.TaskReminders.Add(formTask);
                _db.SaveChanges();
                TempData["success"] = "Your annotation has been created successfully.";
                return RedirectToAction("Index");
            }
            return View(formTask);
        }
    }
}
