using Microsoft.AspNetCore.Mvc;
using MyCalendar.Data;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace MyCalendar.Controllers
{
    //MoonPhaseController is a controller used to handle the MoonPhase section of the app.
    public class MoonPhaseController : Controller
    {

        private readonly ApplicationDbContext _db; // DInjected in constructor. Main DbContext.
        private readonly HttpClient _httpClient; //DInjected in constructor. Main client for http requests.

        public MoonPhaseController(ApplicationDbContext db, IHttpClientFactory httpClientFactory)
        {
            this._db = db; // DInjected in constructor. Main DbContext.
            this._httpClient = httpClientFactory.CreateClient(); //DInjected in constructor. Main client for http requests.
        }

        public IActionResult Index()
        {
            return View();
        }


        //Asks IPinfo API information about user location based on IP.
        //Returns country, region, city.
        public async Task<IActionResult> GetClientLocation()
        {
            // Get the IP address of the remote client
            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
            if (remoteIpAddress == "::1")
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

        public async Task <IActionResult> getMoonInfo(string country)
        {

        }
    }
}
