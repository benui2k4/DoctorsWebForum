using Doctors_Web_Forum.BLL.IServices;
using Doctors_Web_Forum.DAL.Data;
using Doctors_Web_Forum.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctors_Web_Forum.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DataDBContext _dataDBContext;

        public UserService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, DataDBContext dataDBContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dataDBContext = dataDBContext;
        }

        public async Task<(IEnumerable<User> users, Paginate pager)> GetAllUsersAsync(int pg, int pageSize = 5, string searchTerm = "")
        {
            if (pg <= 0) pg = 1;

            var usersQuery = _dataDBContext.Users.AsQueryable();

            // Tìm kiếm theo từ khóa (nếu có)
            if (!string.IsNullOrEmpty(searchTerm))
            {
                usersQuery = usersQuery.Where(u => u.UserName.Contains(searchTerm) || u.Email.Contains(searchTerm));
            }

            usersQuery = usersQuery.OrderByDescending(u => u.Id);

            int recsCount = await usersQuery.CountAsync();

            var pager = new Paginate(recsCount, pg, pageSize);

            int recSkip = (pg - 1) * pageSize;

            // Lấy dữ liệu người dùng với phân trang
            var data = await usersQuery.Skip(recSkip).Take(pageSize).ToListAsync();

            return (data, pager);
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<bool> CreateUserAsync(User model, string role)
        {
            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                PasswordHash = model.PasswordHash,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                Status = model.Status,
                Role = role
            };

            var result = await _userManager.CreateAsync(user);

            if (result.Succeeded)
            {
                // Gán vai trò cho người dùng
                await _userManager.AddToRoleAsync(user, role);
                return true;
            }

            return false;
        }


        public async Task<bool> UpdateUserAsync(User user)
        {
            var existingUser = await _userManager.FindByIdAsync(user.Id);
            if (existingUser == null) return false;

            // Cập nhật các trường trong model của User
            existingUser.FullName = user.FullName;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.Address = user.Address;
            existingUser.Status = user.Status;
            existingUser.LastLogin = user.LastLogin;

            // Nếu có thay đổi tên người dùng hoặc email, cập nhật tương ứng
            if (existingUser.UserName != user.UserName)
            {
                existingUser.UserName = user.UserName;
            }

            if (existingUser.Email != user.Email)
            {
                existingUser.Email = user.Email;
            }


            var result = await _userManager.UpdateAsync(existingUser);
            return result.Succeeded;
        }


        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> AddUserToRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return Enumerable.Empty<string>(); // Trả về danh sách rỗng nếu người dùng không tồn tại

            var roles = await _userManager.GetRolesAsync(user);
            return roles ?? Enumerable.Empty<string>();  // Nếu không có roles, trả về danh sách rỗng
        }


        public async Task<IEnumerable<string>> GetAllRolesAsync()
        {
            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            return roles;
        }
    }
}
