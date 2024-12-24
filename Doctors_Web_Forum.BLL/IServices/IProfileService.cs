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
        Task<Profile> GetProfileByUserIdAsync(string userId);
        Task<bool> UpdateProfileAsync(Profile profile);
        Task<bool> CreateProfileAsync(string userId);
    }
}
