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
        private readonly HttpClient _httpClient; //DInjected in constructor. Main client for http requests.

        public UserSectionController(ApplicationDbContext db, IHttpClientFactory httpClientFactory)
        {
            this._db = db;
            this._httpClient = httpClientFactory.CreateClient();
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


        //TEST!!!!
        public async Task<IActionResult> GetClientLocation()
        {
            // Get the IP address of the remote client
            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
            if(remoteIpAddress == "::1")
            {
                remoteIpAddress = "23.130.205.199";
            }

            // Construct the URL for the GeoIP service
            var url = "http://www.ipinfo.io/" + remoteIpAddress + "?token=43471cd5cb293b";

            try
            {
                // Send a request to the GeoIP service
                // Send a request to the GeoIP service
                var response = await _httpClient.GetStringAsync(url);

                // Parse the JSON response
                var locationData = JObject.Parse(response);

                // Extract location information (the exact fields depend on the GeoIP service you're using)
                var country = locationData["country"].ToString();
                var region = locationData["region"].ToString();
                var city = locationData["city"].ToString();

                // Create a response object
                var clientLocation = new
                {
                    Country = country,
                    Region = region,
                    City = city
                };

                // Return the location data in the response
                return Ok(clientLocation);
            }
            catch (HttpRequestException e)
            {
                // Handle any errors that occur during the request
                return StatusCode(500, "Error accessing the GeoIP service: " + e.Message);
            }
        }
    }
}
