    using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctors_Web_Forum.DAL.Models
{
    [Table("Questions")]
    public class Question
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } 

        [ForeignKey("UserId")]
        public virtual User? User { get; set; } 

        [Required]
        public int TopicId { get; set; } 

        [ForeignKey("TopicId")]
        public virtual Topic? Topic { get; set; } 

        [Required(ErrorMessage = "Question text is required.")]
        [StringLength(1000, ErrorMessage = "Question cannot exceed 1000 characters.")]
        public string QuestionText { get; set; }

        public string? Description {  get; set; }
        public DateTime PostDate { get; set; } = DateTime.Now;

        public bool Status { get; set; } = true; 

        public virtual ICollection<Answer>? Answers { get; set; }
    }
}
