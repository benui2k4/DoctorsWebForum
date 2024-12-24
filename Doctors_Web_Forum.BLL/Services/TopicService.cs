using Doctors_Web_Forum.BLL.IServices;
using Doctors_Web_Forum.DAL.Data;
using Doctors_Web_Forum.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctors_Web_Forum.BLL.Services
{
    public class TopicService : ITopicService
    {
        //  declare DbContext

        private readonly DataDBContext _dataDBContext;



        // using DbContext

        public TopicService(DataDBContext dataDBContext)
        {
            _dataDBContext = dataDBContext;
        }




        // GetAll Topics

        public async Task<(IEnumerable<Topic> topics, Paginate pager)> GetAllTopicsAsync(int pg, int pageSize = 5, string searchTerm = "")
        {
            if (pg <= 0) pg = 1;


            var topicsQuery = _dataDBContext.Topics.AsQueryable();


            if (!string.IsNullOrEmpty(searchTerm))
            {
                topicsQuery = topicsQuery.Where(t => t.TopicName.Contains(searchTerm));
            }


            topicsQuery = topicsQuery.OrderByDescending(t => t.Id);


            int recsCount = await topicsQuery.CountAsync();


            var pager = new Paginate(recsCount, pg, pageSize);


            int recSkip = (pg - 1) * pageSize;


            var data = await topicsQuery.Skip(recSkip).Take(pageSize).ToListAsync();


            return (data, pager);
        }

        // GetTopicById

        public async Task<Topic> GetTopicByIdAsync(int id)
        {
            return await _dataDBContext.Topics.FindAsync(id);
        }

        // Add Topic New 

        public async Task AddTopicAsync(Topic topic)
        {
            if (topic == null)
            {
                throw new ArgumentNullException(nameof(topic), "Topic cannot be null.");
            }

            await _dataDBContext.Topics.AddAsync(topic);
            await _dataDBContext.SaveChangesAsync();
        }

        // Update Topic 

        public async Task UpdateTopicAsync(Topic topic)
        {
            if (topic == null)
            {
                throw new ArgumentNullException(nameof(topic), "Topic cannot be null.");
            }

            // Tìm đối tượng hiện tại trong cơ sở dữ liệu
            var existingTopic = await _dataDBContext.Topics.FindAsync(topic.Id);
            if (existingTopic == null)
            {
                throw new KeyNotFoundException($"Topic with ID {topic.Id} not found.");
            }

            // Cập nhật các thuộc tính
            existingTopic.TopicName = topic.TopicName;
            existingTopic.Description = topic.Description;
            existingTopic.Status = topic.Status;

            // Lưu thay đổi
            await _dataDBContext.SaveChangesAsync();
        }


        // Remove Topic

        public async Task<bool> DeleteTopicAsync(int id)
        {
            var topic = await _dataDBContext.Topics.FindAsync(id);
            if (topic == null)
            {
                return false; // Không tìm thấy chủ đề
            }

            _dataDBContext.Topics.Remove(topic);
            await _dataDBContext.SaveChangesAsync();
            return true;  // Xóa thành công
        }
    }
}
