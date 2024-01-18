using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyCalendar.Models
{
    // CalendarAnnotationViewModel represents a CalendarAnnotation object in a way that can be usable for Views to operate with.
    public class CalendarAnnotationViewModel
    {
        // Unique identifier for the calendar annotation
        [Key]
        public int IdCalendar { get; set; }

        // Title of the annotation, required and limited to 40 characters
        [Required]
        [StringLength(40)]
        public string Title { get; set; }

        // Optional description, limited to 250 characters
        [StringLength(250)]
        public string Description { get; set; }

        // Date of the annotation, required field
        [Required]
        public DateTime Date { get; set; }

        // User ID associated with this annotation, required field
        [Required]
        public int UserId { get; set; }

    }
}
