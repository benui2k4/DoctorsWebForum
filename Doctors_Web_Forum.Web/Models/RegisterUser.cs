using System.ComponentModel.DataAnnotations;

namespace Doctors_Web_Forum.Web.Models
{
    public class RegisterUser
    {
        [Required(ErrorMessage = "Please enter email to register!"), EmailAddress(ErrorMessage = "Invalid email address!")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Please enter Username!")]
        [StringLength(50, ErrorMessage = "Username must be between 3 and 50 characters.", MinimumLength = 3)]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Please enter your full name!")]
        [StringLength(100, ErrorMessage = "Full name must be between 3 and 100 characters.", MinimumLength = 3)]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "Please enter a phone number!")]
        [RegularExpression(@"^\+?[0-9]{7,15}$", ErrorMessage = "Invalid phone number format!")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter an address!")]
        [StringLength(200, ErrorMessage = "Address must be between 5 and 200 characters.", MinimumLength = 5)]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Please enter a password!")]
        [StringLength(100, ErrorMessage = "Password must be at least 6 characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
