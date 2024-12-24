using Doctors_Web_Forum.DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Doctors_Web_Forum.BLL.IServices
{
    public interface IAnswerService
    {
        
        Task<IEnumerable<Answer>> GetAllAnswersAsync();
        Task<Answer> GetAnswerByIdAsync(int id);
        Task<IEnumerable<Answer>> GetAnswersByQuestionIdAsync(int questionId);
        Task<Answer> CreateAnswerAsync(Answer answer);
        Task<Answer> UpdateAnswerAsync(int id, string answerText);
        Task<bool> DeleteAnswerAsync(int id);
    }
}
