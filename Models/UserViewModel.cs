using System.ComponentModel.DataAnnotations;

namespace MyCalendar.Models
{
    // Represents a user in the system in a way that can be usable for Views to operate with.
    public class UserViewModel
    {
        // Unique identifier for the user
        [Key]
        public int IdUser { get; set; }

        // Username of the user, required and restricted to a maximum length of 40 characters
        [Required]
        [StringLength(40)]
        public string Username { get; set; }

        // Email address of the user, required and limited to a maximum length of 250 characters
        [StringLength(250)]
        [Required]
        public string Email { get; set; }

        // Used for confirming the email, must match the Email field. The "Compare" DataAnnotation checks if this field matches with its first argument.
        [Compare("Email", ErrorMessage = "The email and confirmation email do not match.")]
        public string ConfirmEmail { get; set; }

        // Password for the user, required with a minimum length of 6 characters and maximum length of 250 characters
        [StringLength(250)]
        [MinLength(6)]
        [Required]
        public string Password { get; set; }

        // Date when the user subscribed, required field
        [Required]
        public DateTime SubscriptionDate { get; set; }
    }
}