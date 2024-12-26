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

        // Get info profile of users by UserId
        public async Task<Profile> GetProfileByUserIdAsync(string userId)
        {
            return await _context.Profiles
                .Include(p => p.User) // Include user information if needed
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        // Cập nhật thông tin profile
        public async Task<bool> UpdateProfileAsync(Profile profile)
        {
            // Make sure UserId is not null when updating
            if (string.IsNullOrEmpty(profile.UserId))
            {
                return false; // Returns false if UserId is not provided
            }

            // Find the profile that needs updating
            var existingProfile = await _context.Profiles
                .FirstOrDefaultAsync(p => p.UserId == profile.UserId);

            if (existingProfile == null)
            {
                // If the old profile is not found, create a new one
                return false;
            }

            // Update profile information
            existingProfile.FullName = profile.FullName;
            existingProfile.Contact = profile.Contact;
            existingProfile.Phone = profile.Phone;
            existingProfile.Address = profile.Address;
            existingProfile.Status = profile.Status;
            existingProfile.Picture = profile.Picture;

            // Update FullName in the User table
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == profile.UserId);
            if (user != null)
            {
                user.FullName = profile.FullName;
                _context.Users.Update(user);
            }

            // Save changes to the database
            _context.Profiles.Update(existingProfile);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }


        // Create a new profile for the user if you don't have one
        public async Task<bool> CreateProfileAsync(string userId)
        {
            // Kiểm tra xem profile đã tồn tại chưa
            var existingProfile = await _context.Profiles
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (existingProfile != null)
            {
                return false; // If the profile already exists, do not create a new one
            }

            // Create a new profile with default values
            var newProfile = new Profile
            {
                UserId = userId, // Make sure UserId is assigned
                FullName = "New User", 
                Contact = "No Contact", 
                Phone = "No Phone", 
                Address = "No Address", 
                Status = true, 
                Picture = null 
            };

            _context.Profiles.Add(newProfile);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

    }
}
