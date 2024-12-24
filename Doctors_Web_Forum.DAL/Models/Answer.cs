using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctors_Web_Forum.DAL.Models
{
    [Table("Answers")]
    public class Answer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } // Foreign Key đến bảng Users

        [ForeignKey("UserId")]
        public virtual User? User { get; set; } // Navigation property đến User

        [Required]
        public int QuestionId { get; set; } // Foreign Key đến bảng Questions

        [ForeignKey("QuestionId")]
        public virtual Question? Question { get; set; } // Navigation property đến Question

        [Required(ErrorMessage = "Answer text is required.")]
        [StringLength(2000, ErrorMessage = "Answer cannot exceed 2000 characters.")]
        public string? AnswerText { get; set; }

        public DateTime PostedDate { get; set; } = DateTime.Now;

        public bool Status { get; set; } = true;

    }
}
