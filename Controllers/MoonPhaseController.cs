using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyCalendar.Data;
using MyCalendar.Models;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace MyCalendar.Controllers
{
    //MoonPhaseController is a controller used to handle the MoonPhase section of the app.
    public class MoonPhaseController : Controller
    {

        private readonly ApplicationDbContext _db; // DInjected in constructor. Main DbContext.
        private readonly HttpClient _httpClient; //DInjected in constructor. Main client for http requests.
        private readonly WeatherApiSettings _weatherApiSettings; //DInjected in constructor. Object represents settings of WeatherAPI in appsettings.json
        private readonly GeoIpApiSettings _geoipApiSettings; //DInjected in constructor. Object represents settings of GeoIpAPI in appsettings.json

        public MoonPhaseController(ApplicationDbContext db, IHttpClientFactory httpClientFactory, IOptions<WeatherApiSettings> weatherApiSettings, IOptions<GeoIpApiSettings> geoipApiSettings)
        {
            this._db = db;
            this._httpClient = httpClientFactory.CreateClient(); 
            this._weatherApiSettings = weatherApiSettings.Value;
            this._geoipApiSettings = geoipApiSettings.Value;
        }

        public async Task<IActionResult> Index()
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
            var url = $"{_geoipApiSettings.IpAPIUrl}{remoteIpAddress}?token={_geoipApiSettings.IpAPIKey}";

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
                return Ok(clientLocation.City);
            }
            catch (HttpRequestException e)
            {
                // Handle any errors that occur during the request
                return StatusCode(500, "Error accessing the GeoIP service: " + e.Message);
            }
        }

        // Asks WeatherAPI info about current weather on a defined place
        public async Task <IActionResult> GetWeatherInfo(string place)
        {
            //Writing request
            string apiUrl = $"{_weatherApiSettings.HistoryUrl}?key={_weatherApiSettings.WeatherAPIKey}&q={place}&dt={DateTime.Now.ToString("yyyy-MM-dd")}&hour={DateTime.Now.Hour}";

            //Awaiting response from WeatherAPI
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

            //Creating response for current API
            string jsonString = await response.Content.ReadAsStringAsync();

            //Giving back error in case something goes wrong
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(jsonString);
            }

            return Ok(jsonString);
        }
    }
}
