using Doctors_Web_Forum.DAL.Models;
using Doctors_Web_Forum.DAL.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace Doctors_Web_Forum.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    


    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl});
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(loginVM.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Incorrect email or password!");
                    return View(loginVM);
                }

                var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);

                if (result.Succeeded)
                {
                    // Cập nhật thời gian đăng nhập cuối
                    user.LastLogin = DateTime.UtcNow;

                    // Lưu thông tin người dùng với thời gian đăng nhập mới
                    var updateResult = await _userManager.UpdateAsync(user);
                    if (!updateResult.Succeeded)
                    {
                        TempData["error"] = "Unable to update login time!";
                        return RedirectToAction("Login");
                    }

                    TempData["success"] = "Login successful!";
                    return Redirect(loginVM.ReturnUrl ?? "/Admin/Dashboard/Index");
                }

                ModelState.AddModelError("", "Incorrect email or password!");
            }

            return View(loginVM);
        }






        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Tạo đối tượng User mới
                User newUser = new User
                {
                    UserName = model.Username,
                    Email = model.Email,
                    Role = "Admin"
                };

                // Mã hóa mật khẩu trước khi lưu
                var result = await _userManager.CreateAsync(newUser, model.Password);

                if (result.Succeeded)
                {
                    TempData["success"] = "Admin created successfully!";
                    return RedirectToAction("Login", "Account", new { area = "Admin" });
                }

                
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            
            return View(model);
        }


           
        public async Task<IActionResult> Logout(string returnUrl = "/Admin/Account/Login")
        {
            await _signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }

    }
}
