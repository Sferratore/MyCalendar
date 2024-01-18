using Microsoft.AspNetCore.Mvc;
using MyCalendar.Data;
using MyCalendar.Models;

namespace MyCalendar.Controllers
{
    // UserSectionController manages user-specific actions and views in the web application.
    // This controller handles the user's personal section, which includes calendar management.
    public class UserSectionController : Controller
    {
        private readonly ApplicationDbContext _db; // DInjected in constructor. Main DbContext.

        public UserSectionController(ApplicationDbContext db)
        {
            this._db = db;
        }

        // Default action for UserSection. Returns Index view of UserSection.
        public IActionResult Index()
        {
            return View();
        }

        // Retrieves and displays user-specific calendar annotations in a list.
        // Calendar annotations are filtered based on the logged-in user's ID and sorted by date.
        public IActionResult Calendar()
        {
            IEnumerable<CalendarAnnotation> objCalendarAnnotationsList = _db.CalendarAnnotations
                .Where(a => a.User.IdUser == HttpContext.Session.GetInt32("loggedInId"))
                .OrderBy(a => a.Date);
            return View(objCalendarAnnotationsList);
        }

        // Displays the view for creating a new calendar annotation.
        public IActionResult Create()
        {
            CalendarAnnotationViewModel newCalVm = new CalendarAnnotationViewModel();  //Creation of new calendar annotation to use for the process.
            newCalVm.UserId = _db.Users.First(u => u.IdUser == HttpContext.Session.GetInt32("loggedInId")).IdUser; //Setting CalendarAnnotation's UserId
            return View(newCalVm);
        }

        // Processes the creation of a new calendar annotation.
        // Accepts a CalendarAnnotation model from the form submission.
        // Validates the model and adds the new annotation to the database.
        // Redirects to the Calendar view on successful creation.
        [HttpPost]
        public IActionResult Create(CalendarAnnotationViewModel formAnnotation)
        {
            if (ModelState.ErrorCount == 0)
            {
                //Creating CalendarAnnotation object to add to db from CalendarAnnotationViewModel.
                //Adding all info. Excluding IdCalendar which is auto-generated and User which is derived from UserId.
                CalendarAnnotation toAdd = new CalendarAnnotation();
                toAdd.UserId = formAnnotation.UserId;
                toAdd.Date = formAnnotation.Date;
                toAdd.Description = formAnnotation.Description;
                toAdd.Title = formAnnotation.Title;
                _db.CalendarAnnotations.Add(toAdd);
                _db.SaveChanges();
                TempData["successForAnnotation"] = "Your annotation has been created successfully.";
                return RedirectToAction("Calendar");
            }
            return View(formAnnotation);
        }

        // Displays the edit view for a specific calendar annotation.
        // The annotation to be edited is identified by its idCalendar parameter.
        public IActionResult Edit(int idCalendar)
        {
            CalendarAnnotation toEdit = _db.CalendarAnnotations.First(a => a.IdCalendar == idCalendar);
            return View(toEdit);
        }

        // Processes the editing of a specific calendar annotation.
        // Updates the annotation in the database based on the submitted model.
        // Redirects to the Calendar view on successful editing.
        [HttpPost]
        public IActionResult Edit(CalendarAnnotation formAnnotation)
        {
            // ModelState check compensates for expected single error due to user field in CalendarAnnotation model.
            if (ModelState.ErrorCount == 1)
            {
                _db.CalendarAnnotations.Update(formAnnotation);
                _db.SaveChanges();
                TempData["successForAnnotation"] = "Your annotation has been edited successfully.";
                return RedirectToAction("Calendar");
            }
            return View(formAnnotation);
        }

        // Handles the deletion of a specific calendar annotation.
        // The annotation to be deleted is identified by its idCalendar parameter.
        // Deletes the annotation from the database and redirects to the Calendar view.
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
