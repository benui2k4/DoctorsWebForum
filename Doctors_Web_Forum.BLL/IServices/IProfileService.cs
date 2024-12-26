using Doctors_Web_Forum.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctors_Web_Forum.BLL.IServices
{
    public interface IProfileService
    {
        Task<Profile> GetProfileByUserIdAsync(string userId);   // Method Get Profile By User Id
        Task<bool> UpdateProfileAsync(Profile profile);         // Method Update Profile
        Task<bool> CreateProfileAsync(string userId);           // Method Create New Profile
    }
}
