using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctors_Web_Forum.DAL.Models
{
    [Table("AdminLogs")]
    public class AdminLog

    {
        [Key]
        public int Id { get; set; }
        public int AdminId { get; set; }
        public string? Action { get; set; }
        public DateTime ActionTime { get; set; } = DateTime.Now;
    }
}
