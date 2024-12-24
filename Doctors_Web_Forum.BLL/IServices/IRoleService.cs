using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctors_Web_Forum.BLL.IServices
{
    public interface IRoleService
    {
        Task<IEnumerable<IdentityRole>> GetAllRolesAsync();
        Task<IdentityRole> GetRoleByIdAsync(string id);
        Task<bool> CreateRoleAsync(string roleName);
        Task<bool> UpdateRoleAsync(string id, string newRoleName);
        Task<bool> DeleteRoleAsync(string id);
    }
}
