﻿using Microsoft.AspNetCore.Mvc;
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

        // Returns Index view which is the main one that displays data.
        public async Task<IActionResult> Index()
        {
            // Call the GetMoonInfo method to retrieve moon-related information
            IActionResult moonInfoResult = await GetMoonInfo();

            // Check if the result is an OkObjectResult
            if (moonInfoResult is OkObjectResult okMoonInfoResult)
            {
                // Parse the JSON response from the OkObjectResult
                var moonData = JObject.Parse(okMoonInfoResult.Value.ToString());

                // Create a new instance of the MoonDataViewModel to store moon-related data
                MoonDataViewModel moonDataVw = new MoonDataViewModel();

                // Extract moon-related information and populate the MoonDataViewModel
                moonDataVw.Moonrise = moonData["moon_status"]["moonrise"].ToString();
                moonDataVw.Moonset = moonData["moon_status"]["moonset"].ToString();
                moonDataVw.MoonPhase = moonData["moon_status"]["moon_phase"].ToString();
                moonDataVw.MoonIllumination = moonData["moon_status"]["moon_illumination"].ToString();
                moonDataVw.MoonImgUrl = moonData["moon_status"]["moon_image"].ToString();

                // Return the MoonDataViewModel to the corresponding view
                return View(moonDataVw);
            }

            // If the result is not OkObjectResult, return a BadRequest response
            return BadRequest(moonInfoResult);
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

        private async Task<IActionResult> GetMoonInfo()
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

                    // Extract astro details
                    var astroDetails = weatherData["forecast"]["forecastday"][0]["astro"];

                    //Create moonImageUrl
                    string moonImageUrl = string.Empty;
                    switch (astroDetails["moon_phase"].ToString())
                    {
                        case "New Moon":
                            moonImageUrl = "./imgs/newmoon.png";
                            break;
                        case "First Quarter":
                            moonImageUrl = "./imgs/firstquarter.png";
                            break;
                        case "Full Moon":
                            moonImageUrl = "./imgs/fullmoon.png";
                            break;
                        case "Third Quarter":
                            moonImageUrl = "./imgs/thirdquarter.png";
                            break;
                        case "Waning Crescent":
                            moonImageUrl = "./imgs/waningcrescent.png";
                            break;
                        case "Waning Gibbous":
                            moonImageUrl = "./imgs/waninggibbous.png";
                            break;
                        case "Waxing Crescent":
                            moonImageUrl = "./imgs/waxingcrescent.png";
                            break;
                        case "Waxing Gibbous":
                            moonImageUrl = "./imgs/waxinggibbous.png";
                            break;
                        default:
                            moonImageUrl = "./imgs/fullmoon.png";
                            break;
                    }

                    // Construct the new JSON object with the needed data
                    var moonStatus = new JObject
                    {
                        ["moon_status"] = new JObject
                        {
                            ["moonrise"] = astroDetails["moonrise"],
                            ["moonset"] = astroDetails["moonset"],
                            ["moon_phase"] = astroDetails["moon_phase"],
                            ["moon_illumination"] = astroDetails["moon_illumination"],
                            ["moon_image"] = moonImageUrl
                        }
                    };

                    // Return Ok status with data
                    return Ok(moonStatus);
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
