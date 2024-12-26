using Doctors_Web_Forum.Web.Models;
using Doctors_Web_Forum.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Doctors_Web_Forum.Web.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<User> _userManager;

        public RegisterController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        // GET: Register
        [HttpGet]
        public IActionResult Index()
        {
            return View(new RegisterUser()); // Truyền model vào view
        }

        // POST: Register
        [HttpPost]
        public async Task<IActionResult> Index(RegisterUser model)
        {
            if (!ModelState.IsValid) // Kiểm tra dữ liệu có hợp lệ không
            {
                return View(model); // Trả về form với thông báo lỗi
            }

            // Xử lý đăng ký tài khoản
            var newUser = new User
            {
                UserName = model.Username,
                Email = model.Email,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                Role = "Doctor"
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (result.Succeeded)
            {
                TempData["success"] = "Registration successful!";
                return RedirectToAction("Index"); // Reset form sau khi thành công
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }


    }
}