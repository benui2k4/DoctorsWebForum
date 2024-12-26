using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Doctors_Web_Forum.BLL.IServices
{
    public interface IRoleService
    {
        Task<IEnumerable<IdentityRole>> GetAllRolesAsync();            // Method Get All Roles
        Task<IdentityRole> GetRoleByIdAsync(string id);                // Method Get Role By Id
        Task<bool> CreateRoleAsync(string roleName);                   // Method Create New Role
        Task<bool> UpdateRoleAsync(string id, string newRoleName);     // Method Update Role
        Task<bool> DeleteRoleAsync(string id);                         // Method Remove Role (true/false)
    }
}
