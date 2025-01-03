﻿using Doctors_Web_Forum.BLL.IServices;
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
    public class QuestionService : IQuestionService
    {
        private readonly DataDBContext _dataDBContext;  // Đảm bảo tên đúng cho biến context

        // Constructor nhận DataDBContext
        public QuestionService(DataDBContext dataDBContext)
        {
            _dataDBContext = dataDBContext;  // Sửa lỗi sai tên biến trong constructor
        }

        // Lấy thông tin người đăng câu hỏi, chủ đề và câu trả lời (tái sử dụng cho nhiều phương thức)
        private IQueryable<Question> GetQuestionsWithRelatedEntities()
        {
            return _dataDBContext.Questions
                .Include(q => q.User)  // Lấy thông tin người đăng câu hỏi
                .Include(q => q.Topic)  // Lấy thông tin chủ đề câu hỏi
                .Include(q => q.Answers);  // Lấy danh sách câu trả lời của câu hỏi
        }

        // Lấy tất cả câu hỏi với phân trang và tìm kiếm
        public async Task<(IEnumerable<Question> questions, Paginate pager)> GetAllQuestionsAsync(int pg, int pageSize = 5, string searchTerm = "")
        {
            if (pg <= 0) pg = 1;

            // Sử dụng phương thức helper để bao gồm các liên kết (nếu cần)
            var questionsQuery = GetQuestionsWithRelatedEntities();

            // Lọc theo từ khóa tìm kiếm
            if (!string.IsNullOrEmpty(searchTerm))
            {
                // Dùng các trường phù hợp từ model Question, ví dụ: QuestionText, Description
                questionsQuery = questionsQuery.Where(q => q.QuestionText.Contains(searchTerm) || q.Description.Contains(searchTerm));
            }

            // Sắp xếp theo thứ tự giảm dần Id
            questionsQuery = questionsQuery.OrderByDescending(q => q.Id);

            // Tổng số bản ghi
            int recsCount = await questionsQuery.CountAsync();

            // Khởi tạo đối tượng phân trang
            var pager = new Paginate(recsCount, pg, pageSize);

            // Bỏ qua và lấy các bản ghi theo trang
            int recSkip = (pg - 1) * pageSize;
            var data = await questionsQuery.Skip(recSkip).Take(pageSize).ToListAsync();

            return (data, pager);
        }

        // Lấy câu hỏi theo Id và bao gồm câu trả lời
        public async Task<Question> GetQuestionByIdAsync(int id)
        {
            return await _dataDBContext.Questions
                .Include(q => q.User)       // Lấy thông tin người đăng câu hỏi
                .Include(q => q.Topic)      // Lấy thông tin chủ đề của câu hỏi
.Include(q => q.Answers)    // Lấy danh sách các câu trả lời
                .ThenInclude(a => a.User)   // Lấy thông tin người trả lời
                .FirstOrDefaultAsync(q => q.Id == id); // Tìm câu hỏi dựa trên ID
        }


        public async Task<bool> AddQuestionAsync(Question question)
        {
            try
            {
                _dataDBContext.Questions.Add(question);
                await _dataDBContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Tạo câu hỏi mới
        public async Task<Question> CreateQuestionAsync(Question question)
        {
            _dataDBContext.Questions.Add(question);
            await _dataDBContext.SaveChangesAsync();
            return question;
        }

        public async Task<(IEnumerable<Question> questions, Paginate pager)> GetRecentPostsAsync(int pg, int pageSize = 10, string searchTerm = "")
        {
            if (pg <= 0) pg = 1;

            var questionsQuery = _dataDBContext.Questions
                .Include(q => q.User)  // Lấy thông tin người đăng câu hỏi
                .Include(q => q.Topic)  // Lấy thông tin chủ đề câu hỏi
                .OrderByDescending(q => q.PostDate);  // Sắp xếp theo ngày đăng, bài mới nhất lên đầu

            // Nếu có tìm kiếm, lọc theo tiêu đề câu hỏi hoặc mô tả
            if (!string.IsNullOrEmpty(searchTerm))
            {
                questionsQuery = (IOrderedQueryable<Question>)questionsQuery.Where(q => q.QuestionText.Contains(searchTerm) || q.Description.Contains(searchTerm));
            }

            // Tính tổng số bản ghi
            int totalRecords = await questionsQuery.CountAsync();
            var pager = new Paginate(totalRecords, pg, pageSize);

            // Lấy câu hỏi theo trang
            var questions = await questionsQuery
                .Skip((pg - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (questions, pager);
        }

        // Cập nhật câu hỏi
        public async Task<Question> UpdateQuestionAsync(int id, string questionText, string description, int topicId)
        {
            var question = await _dataDBContext.Questions.FindAsync(id);
            if (question == null)
            {
                return null;
            }

            // Cập nhật các trường của câu hỏi
            question.QuestionText = questionText;
            question.Description = description;
            question.TopicId = topicId;

            _dataDBContext.Questions.Update(question);
            await _dataDBContext.SaveChangesAsync();

            return question;
        }

        // Kiểm tra sự tồn tại của câu hỏi trước khi xóa
        private async Task<Question> GetQuestionIfExistsAsync(int id)
        {
            return await _dataDBContext.Questions.FindAsync(id);
        }

        // Xóa câu hỏi
        public async Task<bool> DeleteQuestionAsync(int id)
        {
            var question = await GetQuestionIfExistsAsync(id);
            if (question == null)
            {
                return false; // Nếu câu hỏi không tồn tại
            }

            _dataDBContext.Questions.Remove(question);
            await _dataDBContext.SaveChangesAsync();
            return true;
        }
        public async Task<(IEnumerable<Question>, Paginate)> GetQuestionsByTopicAsync(int topicId, int pg, int pageSize)
        {
            if (pg <= 0) pg = 1;

            // Lọc câu hỏi theo topicId
            var questionsQuery = _dataDBContext.Questions
                .Include(q => q.User) // Bao gồm thông tin người đăng
                .Include(q => q.Topic) // Bao gồm thông tin chủ đề
                .Where(q => q.TopicId == topicId); // Lọc theo topicId

            int recsCount = await questionsQuery.CountAsync();
            var pager = new Paginate(recsCount, pg, pageSize);

            // Phân trang
            var questions = await questionsQuery.Skip((pg - 1) * pageSize).Take(pageSize).ToListAsync();

            return (questions, pager);
        }



        public async Task<List<Answer>> GetAnswersByQuestionIdAsync(int questionId)
        {
            return await _dataDBContext.Answers
                .Include(a => a.User) // Bao gồm thông tin người trả lời
                .Where(a => a.QuestionId == questionId) // Lọc theo QuestionId
                .ToListAsync();
        }


    }
}