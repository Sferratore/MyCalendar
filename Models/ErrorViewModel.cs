namespace MyCalendar.Models
{
    // Represents the error details that can be shown in the view
    public class ErrorViewModel
    {
        // The unique ID associated with the HTTP request that generated the error
        public string? RequestId { get; set; }

        // A boolean property to determine whether to show the RequestId in the view.
        // Returns true if the RequestId is not null or empty, indicating an error ID is present.
        //Can be modified to adapt to visibility-of-error rules.
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
