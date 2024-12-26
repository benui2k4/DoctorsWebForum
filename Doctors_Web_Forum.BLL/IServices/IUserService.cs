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
        Task<(IEnumerable<User> users, Paginate pager)> GetAllUsersAsync(int pg, int pageSize = 5, string searchTerm = ""); // Method Get All Users
        Task<User> GetUserByIdAsync(string id);                                      // Method Get User By Id
        Task<bool> CreateUserAsync(User user, string role);                         // Method Create New User With Role
        Task<bool> UpdateUserAsync(User user);                                      // Method Update User
        Task<bool> DeleteUserAsync(string id);                                      // Method Remove User (true/false)
        Task<bool> AddUserToRoleAsync(string userId, string roleName);              // Method Assign Role To User
        Task<IEnumerable<string>> GetUserRolesAsync(string userId);                 // Method Get User Roles
        Task<IEnumerable<string>> GetAllRolesAsync();                               // Method Get All Roles
    }
}
