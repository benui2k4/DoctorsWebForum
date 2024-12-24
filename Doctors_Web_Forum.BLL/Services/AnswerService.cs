using Doctors_Web_Forum.BLL.IServices;
using Doctors_Web_Forum.DAL.Data;
using Doctors_Web_Forum.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctors_Web_Forum.BLL.Services
{
    public class AnswerService : IAnswerService
    {
        private readonly DataDBContext _context;

        public AnswerService(DataDBContext context)
        {
            _context = context;
        }

        // Lấy thông tin câu trả lời và người trả lời (tái sử dụng cho nhiều phương thức)
        private IQueryable<Answer> GetAnswersWithRelatedEntities()
        {
            return _context.Answers
                .Include(a => a.User)  // Lấy thông tin người trả lời
                .Include(a => a.Question); // Lấy thông tin câu hỏi
        }

        // Lấy tất cả câu trả lời
        public async Task<IEnumerable<Answer>> GetAllAnswersAsync()
        {
            return await GetAnswersWithRelatedEntities().ToListAsync();
        }

        // Lấy câu trả lời theo Id
        public async Task<Answer> GetAnswerByIdAsync(int id)
        {
            return await GetAnswersWithRelatedEntities()
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        // Lấy tất cả câu trả lời cho một câu hỏi
        public async Task<IEnumerable<Answer>> GetAnswersByQuestionIdAsync(int questionId)
        {
            return await GetAnswersWithRelatedEntities()
                .Where(a => a.QuestionId == questionId) // Lọc câu trả lời theo QuestionId
                .ToListAsync();
        }

        // Tạo câu trả lời mới
        public async Task<Answer> CreateAnswerAsync(Answer answer)
        {
            _context.Answers.Add(answer);
            await _context.SaveChangesAsync();
            return answer;
        }

        // Cập nhật câu trả lời
        public async Task<Answer> UpdateAnswerAsync(int id, string answerText)
        {
            var answer = await _context.Answers.FindAsync(id);
            if (answer == null)
            {
                return null; // Nếu câu trả lời không tồn tại
            }

            // Cập nhật nội dung câu trả lời
            answer.AnswerText = answerText;

            _context.Answers.Update(answer);
            await _context.SaveChangesAsync();

            return answer;
        }

        // Kiểm tra sự tồn tại của câu trả lời trước khi xóa
        private async Task<Answer> GetAnswerIfExistsAsync(int id)
        {
            return await _context.Answers.FindAsync(id);
        }

        // Xóa câu trả lời
        public async Task<bool> DeleteAnswerAsync(int id)
        {
            var answer = await GetAnswerIfExistsAsync(id);
            if (answer == null)
            {
                return false; // Nếu câu trả lời không tồn tại
            }
                
            _context.Answers.Remove(answer);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
