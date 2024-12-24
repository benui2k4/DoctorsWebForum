using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctors_Web_Forum.DAL.Models
{
    [Table("Profiles")]
    public class Profile
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public virtual User User { get; set; }
        [Required]
        public string FullName { get; set; }
        public string? Picture { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }

        public string? Contact { get; set; }
        [Required]
        public bool Status { get; set; }

        public virtual DoctorProfile? DoctorProfile { get; set; }
    }
}
