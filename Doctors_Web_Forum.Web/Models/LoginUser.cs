using System.ComponentModel.DataAnnotations;

namespace Doctors_Web_Forum.Web.Models
{
    public class LoginUser
    {
        [Required(ErrorMessage = "Please enter email to login !"), EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Please enter password to login !")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public string? ReturnUrl { get; set; }
    }
}