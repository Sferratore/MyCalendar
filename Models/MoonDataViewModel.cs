namespace MyCalendar.Models
{
    // MoonDataViewModel represents moon data in a way that can be usable for Views to operate with.
    public class MoonDataViewModel
    {
        public string Moonrise { get; set; }
        public string Moonset { get; set;}
        public string MoonPhase { get; set;}
        public string MoonIllumination { get; set;}
        public string MoonImgUrl { get; set;}
    }
}
