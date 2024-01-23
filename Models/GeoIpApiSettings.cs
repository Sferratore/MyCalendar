namespace MyCalendar.Models
{
    /*
     * WeatherApiSettings is a class used to store geoip api settings stored in appsettings.json.
     */
    public class GeoIpApiSettings
    {
        public string IpAPIUrl { get; set; } // Url of Ip API used
        public string IpAPIKey { get; set; } // Key of Ip API used
    }
}
