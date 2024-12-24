using Doctors_Web_Forum.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctors_Web_Forum.BLL.IServices
{
    public interface ITopicService
    {
        Task<(IEnumerable<Topic> topics, Paginate pager)> GetAllTopicsAsync(int pg, int pageSize = 5, string searchTerm = "");       // Method GetAll Topics 
        Task<Topic> GetTopicByIdAsync(int id);              // Method GetTopicById 
        Task AddTopicAsync(Topic topic);                    // Method Add Method New
        Task UpdateTopicAsync(Topic topic);                 // Method Update Topic 
        Task<bool> DeleteTopicAsync(int id);                // Method Remove Topic, return (true/false)
    }
}
