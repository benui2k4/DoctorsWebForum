using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctors_Web_Forum.DAL.Models
{
    [Table("Topics")]
    public class Topic
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Topic name is required !")]
        [StringLength(255, ErrorMessage = "Topic name cannot exceed 255 characters.")]
        [Column("topic_name")]
        public string? TopicName { get; set; }
        [Required(ErrorMessage ="Description is required !")]
        [StringLength(255, ErrorMessage = "Description cannot exceed 255 characters.")]
        public string? Description { get; set; }

        [Required]
        public bool Status { get; set; } = true;


        public virtual ICollection<Question>? Questions { get; set; }
    }
}
