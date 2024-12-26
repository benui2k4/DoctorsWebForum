using Doctors_Web_Forum.DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Doctors_Web_Forum.BLL.IServices
{
    public interface IAnswerService
    {
        Task<IEnumerable<Answer>> GetAllAnswersAsync();                      // Method Get All Answers
        Task<Answer> GetAnswerByIdAsync(int id);                            // Method Get Answer By Id
        Task<bool> AddAnswerAsync(Answer answer);                           // Method Add New Answer
        Task<IEnumerable<Answer>> GetAnswersByQuestionIdAsync(int questionId);  // Method Get Answers By Question Id
        Task<Answer> CreateAnswerAsync(Answer answer);                      // Method Create New Answer
        Task<Answer> UpdateAnswerAsync(int id, string answerText);          // Method Update Answer
        Task<bool> DeleteAnswerAsync(int id);                               // Method Remove Answer (true/false)
    }
}
