using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctors_Web_Forum.DAL.Models
{
    [Table("LoginAttempts")]
    public class LoginAttempt
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime AttemptTime { get; set; } = DateTime.Now;
        public bool Status { get; set; }
    }
}
