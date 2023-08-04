using System.ComponentModel.DataAnnotations;

namespace MyCalendar.Models
{
    public class TaskReminder
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(40)]
        public string Title { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }
}
