using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doctors_Web_Forum.DAL.Models
{
    [Table("Users")]
    public class User : IdentityUser
    {
       
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public DateTime RegDate { get; set; } = DateTime.Now;
        public DateTime? LastLogin { get; set; }
        public bool Status { get; set; }
        [Required]
        public string? Role { get; set; }

        public virtual Profile Profile { get; set; }
        public virtual ICollection<Question>? Questions { get; set; } 
        public virtual ICollection<Answer>? Answers { get; set; } 
    }
}
