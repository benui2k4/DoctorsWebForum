using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctors_Web_Forum.DAL.Models.ViewModel
{
    public class AnswerViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }

        public int QuestionId { get; set; }
        public string AnswerText { get; set; }
        public DateTime PostedDate { get; set; }
        public bool Status { get; set; }
        public bool IsCurrentUser { get; set; }
    }

}
