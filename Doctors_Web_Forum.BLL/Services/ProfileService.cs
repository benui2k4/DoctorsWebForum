using Doctors_Web_Forum.DAL.Models;
using Doctors_Web_Forum.DAL.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Doctors_Web_Forum.BLL.IServices;

namespace Doctors_Web_Forum.Services
{
    public class ProfileService : IProfileService
    {
        private readonly DataDBContext _context;

        public ProfileService(DataDBContext context)
        {
            _context = context;
        }

        // Lấy thông tin profile của người dùng theo UserId
        public async Task<Profile> GetProfileByUserIdAsync(string userId)
        {
            return await _context.Profiles
                .Include(p => p.User) // Bao gồm thông tin user nếu cần
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        // Cập nhật thông tin profile
        public async Task<bool> UpdateProfileAsync(Profile profile)
        {
            // Đảm bảo UserId không phải null khi cập nhật
            if (string.IsNullOrEmpty(profile.UserId))
            {
                return false; // Trả về false nếu UserId không được cung cấp
            }

            // Tìm profile cần cập nhật
            var existingProfile = await _context.Profiles
                .FirstOrDefaultAsync(p => p.UserId == profile.UserId);

            if (existingProfile == null)
            {
                // Nếu không tìm thấy profile cũ, tạo mới
                return false;
            }

            // Cập nhật thông tin của profile
            existingProfile.FullName = profile.FullName;
            existingProfile.Contact = profile.Contact;
            existingProfile.Phone = profile.Phone;
            existingProfile.Address = profile.Address;
            existingProfile.Status = profile.Status;
            existingProfile.Picture = profile.Picture;

            // Cập nhật FullName trong bảng User
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == profile.UserId);
            if (user != null)
            {
                user.FullName = profile.FullName;
                _context.Users.Update(user);
            }

            // Lưu thay đổi vào cơ sở dữ liệu
            _context.Profiles.Update(existingProfile);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }


        // Tạo profile mới cho user nếu chưa có
        public async Task<bool> CreateProfileAsync(string userId)
        {
            // Kiểm tra xem profile đã tồn tại chưa
            var existingProfile = await _context.Profiles
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (existingProfile != null)
            {
                return false; // Nếu profile đã tồn tại, không tạo mới
            }

            // Tạo profile mới với giá trị mặc định
            var newProfile = new Profile
            {
                UserId = userId, // Đảm bảo UserId được gán
                FullName = "New User", // Giá trị mặc định
                Contact = "No Contact", // Giá trị mặc định
                Phone = "No Phone", // Giá trị mặc định
                Address = "No Address", // Giá trị mặc định
                Status = true, // Giá trị mặc định, có thể là Active
                Picture = null // Giá trị mặc định hoặc null nếu không có ảnh
            };

            _context.Profiles.Add(newProfile);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

    }
}
