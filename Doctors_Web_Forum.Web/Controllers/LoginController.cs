using Doctors_Web_Forum.Web.Models;
using Doctors_Web_Forum.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Doctors_Web_Forum.Web.Controllers
{
    [Route("login")]
    public class LoginController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public LoginController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index(string returnUrl = "/")
        {
            return View(new LoginUser { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Index(LoginUser model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Trả về view nếu dữ liệu không hợp lệ
            }

            // Tìm kiếm người dùng qua email
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                TempData["error"] = "The email address is not registered.";
                return View(model);
            }

            // Đăng nhập
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

            if (result.Succeeded)
            {
                user.LastLogin = DateTime.UtcNow;
                TempData["success"] = "Login successful!";
                return RedirectToAction("Index", "Home"); 
            }

            TempData["error"] = "Invalid email or password.";
            return View(model);
        }


       
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData["success"] = "You have logged out successfully.";
            return RedirectToAction("Index", "Login");
        }
    }
}
