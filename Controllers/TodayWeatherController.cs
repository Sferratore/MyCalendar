using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyCalendar.Data;
using MyCalendar.Models;
using Newtonsoft.Json.Linq;

namespace MyCalendar.Controllers
{
    public class TodayWeatherController : Controller
    {

        private readonly ApplicationDbContext _db; // DInjected in constructor. Main DbContext.
        private readonly HttpClient _httpClient; //DInjected in constructor. Main client for http requests.
        private readonly WeatherApiSettings _weatherApiSettings; //DInjected in constructor. Object represents settings of WeatherAPI in appsettings.json
        private readonly GeoIpApiSettings _geoipApiSettings; //DInjected in constructor. Object represents settings of GeoIpAPI in appsettings.json

        public TodayWeatherController(ApplicationDbContext db, IHttpClientFactory httpClientFactory, IOptions<WeatherApiSettings> weatherApiSettings, IOptions<GeoIpApiSettings> geoipApiSettings)
        {
            this._db = db;
            this._httpClient = httpClientFactory.CreateClient();
            this._weatherApiSettings = weatherApiSettings.Value;
            this._geoipApiSettings = geoipApiSettings.Value;
        }

        // Returns Index view which is the main one that displays data.
        public async Task<IActionResult> Index()
        {
            // Call the GetProcessedWeatherInfo method to retrieve weather-related information
            IActionResult weatherInfoResult = await GetProcessedWeatherInfo();

            // Check if the result is an OkObjectResult
            if (weatherInfoResult is OkObjectResult okWeatherInfoResult)
            {
                // Parse the JSON response from the OkObjectResult
                var weatherData = JObject.Parse(okWeatherInfoResult.Value.ToString());

                // Create a new instance of the TodayWeatherDataViewModel to store weather-related data
                TodayWeatherDataViewModel weatherDataVw = new TodayWeatherDataViewModel();

                // Extract weather-related information and populate the TodayWeatherDataViewModel
                weatherDataVw.WeatherCondition = weatherData["weather_status"]["weather_condition"].ToString();
                weatherDataVw.AvgTemp_c = weatherData["weather_status"]["avgtemp_c"].ToString();
                weatherDataVw.AvgTemp_f = weatherData["weather_status"]["avgtemp_f"].ToString();
                weatherDataVw.AvgHumidity = weatherData["weather_status"]["avghumidity"].ToString();
                weatherDataVw.AvgUv = weatherData["weather_status"]["avguv"].ToString();
                weatherDataVw.WeatherImgUrl = weatherData["weather_status"]["weather_image"].ToString();

                // Return the TodayWeatherDataViewModel to the corresponding view
                return View(weatherDataVw);
            }

            // If the result is not OkObjectResult, return a BadRequest response
            return BadRequest(weatherInfoResult);
        }



        //Asks IPinfo API information about user location based on IP.
        //Returns country, region, city.
        private async Task<IActionResult> GetClientLocation(string remoteIpAddress)
        {

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
        private async Task<IActionResult> GetWeatherInfo(string place)
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

        private async Task<IActionResult> GetProcessedWeatherInfo()
        {
            // Get the IP address of the remote client
            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
            if (remoteIpAddress == "::1")
            {
                remoteIpAddress = "23.130.205.199";
            }

            //Collecting location data
            IActionResult clientLocationResult = await GetClientLocation(remoteIpAddress);
            if (clientLocationResult is OkObjectResult okClientLocationResult)
            {
                //If response is OK, then we take the location data
                string location = okClientLocationResult.Value as string;
                //Calling GetWeatherInfo to get data about weather
                IActionResult weatherDataResult = await GetWeatherInfo(location);
                if (weatherDataResult is OkObjectResult okWeatherDataResult)
                {
                    //If response is ok, we retrieve data and manipulate it to take what we need
                    // Parse the JSON response
                    var weatherData = JObject.Parse(okWeatherDataResult.Value.ToString());

                    // Extract weather details
                    var weatherDetails = weatherData["forecast"]["forecastday"][0]["day"];

                    //Create weatherImageUrl
                    //NEEEED TO MODIFY URLS ONCE ADDED IMAGES
                    string weatherImageUrl = string.Empty;
                    switch (weatherDetails["condition"]["text"].ToString())
                    {
                        case "Sunny":
                            weatherImageUrl = "./imgs/sunny.png";
                            break;
                        case "Fog":
                            weatherImageUrl = "./imgs/fog.png";
                            break;
                        case "Heavy snow":
                            weatherImageUrl = "./imgs/heavysnow.png";
                            break;
                        case "Patchy rain possible":
                            weatherImageUrl = "./imgs/patchyrainpossible.png";
                            break;
                        case "Light rain shower":
                            weatherImageUrl = "./imgs/lightrainshower.png";
                            break;
                        case "Overcast":
                            weatherImageUrl = "./imgs/overcast.png";
                            break;
                        default:
                            weatherImageUrl = "./imgs/fullmoon.png";
                            break;
                    }

                    // Construct the new JSON object with the needed data
                    var weatherStatus = new JObject
                    {
                        ["weather_status"] = new JObject
                        {
                            ["weather_condition"] = weatherDetails["condition"]["text"],
                            ["avgtemp_c"] = weatherDetails["avgtemp_c"],
                            ["avgtemp_f"] = weatherDetails["avgtemp_f"],
                            ["avghumidity"] = weatherDetails["avghumidity"],
                            ["avguv"] = weatherDetails["uv"],
                            ["weather_image"] = weatherImageUrl
                        }
                    };

                    // Return Ok status with data
                    return Ok(weatherStatus);
                }
                else
                {
                    // Return bad request with result detail
                    return BadRequest(weatherDataResult);
                }
            }

            // Return bad request with result detail
            return BadRequest(clientLocationResult);

        }
    }
}
