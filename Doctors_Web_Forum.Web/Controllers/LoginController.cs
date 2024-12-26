using Doctors_Web_Forum.DAL.Models;
using Doctors_Web_Forum.DAL.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Doctors_Web_Forum.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public LoginController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View(); // Trả về View Index.cshtml (nội dung trang giới thiệu)
        }


        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            var loginVM = new LoginViewModel { ReturnUrl = returnUrl };
            return View(loginVM);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (ModelState.IsValid)
            {
                // Tìm người dùng theo tên đăng nhập
                var user = await _userManager.FindByNameAsync(loginVM.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Tên người dùng không tồn tại.");
                    return View(loginVM);
                }

                // Thực hiện đăng nhập
                var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);

                if (result.Succeeded)
                {
                    // Gán Role "User" nếu chưa có
                    var roles = await _userManager.GetRolesAsync(user);
                    if (!roles.Contains("User"))
                    {
                        await _userManager.AddToRoleAsync(user, "User");
                    }

                    // Cập nhật thời gian đăng nhập cuối (nếu cần)
                    user.LastLogin = DateTime.UtcNow;
                    var updateResult = await _userManager.UpdateAsync(user);

                    if (!updateResult.Succeeded)
                    {
                        TempData["error"] = "Không thể cập nhật thời gian đăng nhập.";
                        return RedirectToAction("Login");
                    }

                    TempData["success"] = "Đăng nhập thành công!";
                    return Redirect(loginVM.ReturnUrl ?? "/Home/Index");
                }
                else
                {
                    ModelState.AddModelError("", "Tên người dùng hoặc mật khẩu không chính xác!");
                }
            }

            return View(loginVM);
        }

    }
}
