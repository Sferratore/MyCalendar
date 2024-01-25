namespace MyCalendar.Models
{
    // TodayWeatherDataViewModel represents weather data in a way that can be usable for Views to operate with.
    public class TodayWeatherDataViewModel
    {
        public string WeatherCondition { get; set; }
        public string AvgTemp_c { get; set; }
        public string AvgTemp_f { get; set; }
        public string AvgHumidity { get; set; }
        public string AvgUv { get; set; }
        public string WeatherImgUrl { get; set; }
    }
}
