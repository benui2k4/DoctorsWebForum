using Doctors_Web_Forum.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctors_Web_Forum.BLL.IServices
{
    public interface IUserService
    {
        Task<(IEnumerable<User> users, Paginate pager )> GetAllUsersAsync(int pg, int pageSize = 5, string searchTerm = "");
        Task<User> GetUserByIdAsync(string id);
        Task<bool> CreateUserAsync(User user ,string role);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(string id);
        Task<bool> AddUserToRoleAsync(string userId, string roleName);
        Task<IEnumerable<string>> GetUserRolesAsync(string userId);

        Task<IEnumerable<string>> GetAllRolesAsync();


    }
}
