using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCalendar.Models
{
    // Represents an annotation in the calendar system
    public class CalendarAnnotation
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

        // Navigation property for the User, indicating which user the annotation belongs to. Uses "UserId" as foreign key.
        [ForeignKey("UserId")]
        [Required]
        public virtual User User { get; set; }
    }
}
