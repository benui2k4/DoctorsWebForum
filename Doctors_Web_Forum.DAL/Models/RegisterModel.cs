using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctors_Web_Forum.DAL.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Please enter email to register !"),EmailAddress]
        public string? Email { get; set; }
        [Required(ErrorMessage ="Please enter Username !")]
        public string ? Username { get; set; }

        [Required(ErrorMessage ="Please enter password to Register !")]
       
        public string? Password { get; set; }
       
    }
}
