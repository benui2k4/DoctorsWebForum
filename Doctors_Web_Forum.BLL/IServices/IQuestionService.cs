using Doctors_Web_Forum.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Doctors_Web_Forum.BLL.IServices
{
    public interface IQuestionService
    {
        
        Task<(IEnumerable<Question> questions, Paginate pager)> GetAllQuestionsAsync(int pg, int pageSize = 5, string searchTerm = "");

        Task<Question> GetQuestionByIdAsync(int id);
        Task<Question> CreateQuestionAsync(Question question);
        Task<Question> UpdateQuestionAsync(int id, string questionText, string description, int topicId);
        Task<bool> DeleteQuestionAsync(int id);
    }
}
