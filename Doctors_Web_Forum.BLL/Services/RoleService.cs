using Doctors_Web_Forum.BLL.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doctors_Web_Forum.BLL.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<IdentityRole>> GetAllRolesAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<bool> CreateRoleAsync(string roleName)
        {
            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
            return result.Succeeded;
        }

        public async Task<bool> DeleteRoleAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return false;

            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded;
        }

        public async Task<IdentityRole> GetRoleByIdAsync(string id)
        {
            return await _roleManager.FindByIdAsync(id);
        }

        public async Task<bool> UpdateRoleAsync(string id, string newRoleName)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return false;

            role.Name = newRoleName;
            var result = await _roleManager.UpdateAsync(role);
            return result.Succeeded;
        }
    }
}
