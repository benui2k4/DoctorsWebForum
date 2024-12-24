using Microsoft.AspNetCore.Identity;
using Doctors_Web_Forum.BLL.IServices;
using Doctors_Web_Forum.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Doctors_Web_Forum.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager , UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // Hiển thị danh sách các role
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        // Tạo role
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                ModelState.AddModelError("", "Role name cannot be empty.");
                return View();
            }

            var role = new IdentityRole(roleName);
            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                TempData["success"] = "Role created successfully!";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Failed to create role.");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                ModelState.AddModelError("", "Invalid role ID.");
                return RedirectToAction("Index");
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                // Kiểm tra xem có người dùng nào đang sử dụng role này không
                var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
                if (usersInRole.Any())
                {
                    TempData["error"] = "Cannot delete role because it is assigned to one or more users.";
                    return RedirectToAction("Index");
                }

                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    TempData["success"] = "Role deleted successfully!";
                }
                else
                {
                    TempData["error"] = "Failed to delete role. " + string.Join(", ", result.Errors.Select(e => e.Description));
                }
            }
            else
            {
                TempData["error"] = "Role not found.";
            }

            return RedirectToAction("Index");
        }





        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return RedirectToAction("Index");
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return RedirectToAction("Index");
            }

            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, string newRoleName)
        {
            if (string.IsNullOrWhiteSpace(newRoleName))
            {
                ModelState.AddModelError("", "Role name cannot be empty.");
                return View();
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                role.Name = newRoleName;
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    TempData["success"] = "Role updated successfully!";
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError("", "Failed to update role.");
            return View();
        }
    }
}
