using Doctors_Web_Forum.BLL.IServices;
using Doctors_Web_Forum.DAL.Data;
using Doctors_Web_Forum.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Doctors_Web_Forum.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private RoleManager<IdentityRole> _roleManager;
        private DataDBContext _dataDBContext;

        public UserController(IUserService userService , RoleManager<IdentityRole> roleManager, DataDBContext dataDBContext) 
        {
            _userService = userService;        
            _roleManager = roleManager;
            _dataDBContext = dataDBContext;
        }


        public async Task<IActionResult> Index(int pg = 1, int pageSize = 5, string searchTerm = "")
        {
            var (users, pager) = await _userService.GetAllUsersAsync(pg, pageSize, searchTerm);

            // Truyền thông tin phân trang vào ViewBag
            ViewBag.Pager = pager;
            ViewBag.SearchTerm = searchTerm;

            return View(users);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var roles = _roleManager.Roles.ToList();
            ViewBag.Roles = new SelectList(roles, "Name", "Name");
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Create(User model, string role)
        {
           
            var roles = _roleManager.Roles.ToList();
            ViewBag.Roles = new SelectList(roles, "Name", "Name");

            //if (ModelState.IsValid)
            
                // Gọi dịch vụ để tạo người dùng, truyền role vào
                var result = await _userService.CreateUserAsync(model, role);

                // Kiểm tra kết quả trả về
                if (result)
                {
                    TempData["success"] = "User created successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error"] = "Failed to create user.";
                }
            

            // Nếu có lỗi thì trả về view hiện tại với model và danh sách role
            return View(model);
        }




        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["error"] = "User ID is invalid.";
                return RedirectToAction("Index");
            }

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                TempData["error"] = "User not found.";
                return RedirectToAction("Index");
            }

            return View(user);
        }


        // GET: Edit
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["error"] = "User ID is invalid.";
                return RedirectToAction("Index");
            }

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                TempData["error"] = "User not found.";
                return RedirectToAction("Index");
            }

            // Get all roles and pass them to the view
            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Name", "Name", user.Role); 

            // Return view with user information and roles
            return View(user);
        }

        // POST: Edit
        [HttpPost]
        public async Task<IActionResult> Edit(User model)
        {
            if (ModelState.IsValid)
            {
                // Update user information
                var result = await _userService.UpdateUserAsync(model);

                // Check if update was successful
                if (result)
                {
                    TempData["success"] = "User updated successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error"] = "Failed to update user.";
                }
            }

            // If validation fails or update fails, return the view with model
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> AddUserToRole(string userId, string roleName)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleName))
            {
                TempData["error"] = "Invalid user or role.";
                return RedirectToAction("Index");
            }

            var result = await _userService.AddUserToRoleAsync(userId, roleName);
            if (result)
            {
                TempData["success"] = "User added to role successfully!";
            }
            else
            {
                TempData["error"] = "Failed to add user to role.";
            }

            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                TempData["error"] = "User ID is invalid.";
                return RedirectToAction("Index");
            }

            var roles = await _userService.GetUserRolesAsync(userId);

            if (roles != null && roles.Any())  // Kiểm tra xem có roles hay không
            {
                ViewBag.Roles = roles;  // Nếu có, gán vào ViewBag để truyền sang view
                return View();  // Hiển thị view với roles
            }
            else
            {
                TempData["error"] = "No roles found for this user.";  // Thông báo nếu không có roles
                return RedirectToAction("Index");  // Quay lại trang chính
            }
        }




        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["error"] = "User ID is invalid.";
                return RedirectToAction("Index");
            }

            var result = await _userService.DeleteUserAsync(id);

            if (result)
            {
                TempData["success"] = "User deleted successfully.";
            }
            else
            {
                TempData["error"] = "Failed to delete user. User not found.";
            }

            return RedirectToAction("Index");
        }


    }
}
