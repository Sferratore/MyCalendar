using System.ComponentModel.DataAnnotations;

namespace MyCalendar.Models
{
    public class User
    {
        [Key]
        public int IdUser { get; set; }
        [Required]
        [StringLength(40)]
        public string Username { get; set; }
        [StringLength(250)]
        [Required]
        public string Email { get; set; }

        [Compare("Email", ErrorMessage = "The email and confirmation email do not match.")]
        public string ConfirmEmail { get; set; }

        [StringLength(250)]
        [MinLength(6)]
        [Required]
        public string Password { get; set; }

        [Required]
        public DateTime SubscriptionDate { get; set; }
    }
}
