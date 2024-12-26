using Doctors_Web_Forum.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Doctors_Web_Forum.BLL.IServices
{
    public interface IQuestionService
    {
        Task<(IEnumerable<Question> questions, Paginate pager)> GetAllQuestionsAsync(int pg, int pageSize = 10, string searchTerm = "");  // Method Get All Questions

        Task<Question> GetQuestionByIdAsync(int id);                              // Method Get Question By Id
        Task<Question> CreateQuestionAsync(Question question);                   // Method Create New Question
        Task<(IEnumerable<Question> questions, Paginate pager)> GetRecentPostsAsync(int pg, int pageSize = 5, string searchTerm = "");  // Method Get Recent Posts
        Task<Question> UpdateQuestionAsync(int id, string questionText, string description, int topicId);  // Method Update Question
        Task<bool> DeleteQuestionAsync(int id);                                  // Method Remove Question (true/false)
        Task<(IEnumerable<Question>, Paginate)> GetQuestionsByTopicAsync(int topicId, int pg, int pageSize);  // Method Get Questions By Topic

        Task<List<Answer>> GetAnswersByQuestionIdAsync(int questionId);          // Method Get Answers By Question Id

        Task<bool> AddQuestionAsync(Question question);                          // Method Add New Question
    }
}
