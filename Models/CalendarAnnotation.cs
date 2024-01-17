using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCalendar.Models
{
    public class CalendarAnnotation
    {
        [Key]
        public int IdCalendar { get; set; }
        [Required]
        [StringLength(40)]
        public string Title { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        [Required]
        public virtual User User { get; set; }
    }
}
