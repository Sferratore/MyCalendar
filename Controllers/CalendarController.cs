using Microsoft.AspNetCore.Mvc;
using MyCalendar.Data;
using MyCalendar.Models;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyCalendar.Controllers
{
    //CalendarController is a controller used to handle operation on the Calendar section of the app.
    public class CalendarController : Controller
    {
        private readonly ApplicationDbContext _db; // DInjected in constructor. Main DbContext.

        public CalendarController(ApplicationDbContext db, IHttpClientFactory httpClientFactory)
        {
            this._db = db; // DInjected in constructor. Main DbContext.
        }


        // Retrieves and displays user-specific calendar annotations in a list.
        // Calendar annotations are filtered based on the logged-in user's ID and sorted by date.
        public IActionResult Index()
        {
            IEnumerable<CalendarAnnotation> objCalendarAnnotationsList = _db.CalendarAnnotations
                .Where(a => a.User.IdUser == HttpContext.Session.GetInt32("loggedInId"))
                .OrderBy(a => a.Date);
            return View(objCalendarAnnotationsList);
        }

        //Returns Error.cshtml view when /Error is called with a GET action.
        //Passes an ErrorViewModel object to display data on view.
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)] //Disables caching of the response to this call.
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
            if (ModelState.IsValid)
            {
                //Creating CalendarAnnotation object to add to db from CalendarAnnotationViewModel.
                //Adding all info. Excluding IdCalendar which is auto-generated and User which is derived from UserId.
                CalendarAnnotation toAdd = new CalendarAnnotation();
                toAdd.UserId = formAnnotation.UserId;
                toAdd.Date = formAnnotation.Date;
                toAdd.Description = formAnnotation.Description;
                toAdd.Title = formAnnotation.Title;

                //Adding to Db
                _db.CalendarAnnotations.Add(toAdd);
                _db.SaveChanges();
                TempData["annotationOperationFeedback"] = "Your annotation has been created successfully.";
                return RedirectToAction("Index");
            }
            return View(formAnnotation);
        }

        // Displays the edit view for a specific calendar annotation.
        // The annotation to be edited is identified by its idCalendar parameter.
        public IActionResult Edit(int idCalendar)
        {
            //Getting annotation
            CalendarAnnotation toEdit = _db.CalendarAnnotations.First(a => a.IdCalendar == idCalendar);
            //Converting it to CalendarAnnotationViewModel
            CalendarAnnotationViewModel toPassToView = new CalendarAnnotationViewModel();
            toPassToView.UserId = toEdit.UserId;
            toPassToView.IdCalendar = idCalendar;
            toPassToView.Date = toEdit.Date;
            toPassToView.Description = toEdit.Description;
            toPassToView.Title = toEdit.Title;
            //Passing it to view
            return View(toPassToView);
        }

        // Processes the editing of a specific calendar annotation.
        // Updates the annotation in the database based on the submitted model.
        // Redirects to the Calendar view on successful editing.
        [HttpPost]
        public IActionResult Edit(CalendarAnnotationViewModel formAnnotation)
        {
            // ModelState check compensates for expected single error due to user field in CalendarAnnotation model.
            if (ModelState.IsValid)
            {
                //Creating CalendarAnnotation object to update into db from CalendarAnnotationViewModel.
                //Adding all info. Excluding User which is derived from UserId.
                CalendarAnnotation toUpdate = new CalendarAnnotation();
                toUpdate.IdCalendar = formAnnotation.IdCalendar;
                toUpdate.UserId = formAnnotation.UserId;
                toUpdate.Date = formAnnotation.Date;
                toUpdate.Description = formAnnotation.Description;
                toUpdate.Title = formAnnotation.Title;
                //Saving new data to DB.
                _db.CalendarAnnotations.Update(toUpdate);
                _db.SaveChanges();
                //Redirect to Calendar.cshtml with feedback info.
                TempData["annotationOperationFeedback"] = "Your annotation has been edited successfully.";
                return RedirectToAction("Index");
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
            TempData["annotationOperationFeedback"] = "Your annotation has been deleted successfully.";
            return RedirectToAction("Index");
        }

    }
}
