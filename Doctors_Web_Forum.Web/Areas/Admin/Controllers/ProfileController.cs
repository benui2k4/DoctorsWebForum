using Doctors_Web_Forum.DAL.Models;
using Doctors_Web_Forum.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Doctors_Web_Forum.BLL.IServices;
using Microsoft.Extensions.Logging;
using Doctors_Web_Forum.Models;

namespace Doctors_Web_Forum.Controllers
{
    [Area("Admin")]
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(IProfileService profileService, UserManager<User> userManager, SignInManager<User> signInManager, ILogger<ProfileController> logger)
        {
            _profileService = profileService;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        // Hiển thị thông tin profile của người dùng
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var profile = await _profileService.GetProfileByUserIdAsync(user.Id);
            if (profile == null)
            {
                // Nếu không có profile thì tạo mới
                await _profileService.CreateProfileAsync(user.Id);
                profile = await _profileService.GetProfileByUserIdAsync(user.Id);
            }

            return View(profile);
        }

        // Hiển thị trang chỉnh sửa profile
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var profile = await _profileService.GetProfileByUserIdAsync(user.Id);
            if (profile == null)
            {
                return NotFound();
            }

            return View(profile);
        }

        // Chỉnh sửa thông tin profile
        [HttpPost]
        public async Task<IActionResult> Edit(Profile profile, IFormFile? pictureFile)
        {
            // Lấy thông tin người dùng hiện tại
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();  // Nếu không tìm thấy người dùng, trả về lỗi
            }

            // Đảm bảo UserId được gán từ user hiện tại
            profile.UserId = user.Id;

            // Cập nhật thông tin FullName trong bảng User
            user.FullName = profile.FullName;
            await _userManager.UpdateAsync(user); // Cập nhật User

            // Xử lý file ảnh tải lên
            if (pictureFile != null && pictureFile.Length > 0)
            {
                // Đặt thư mục để lưu ảnh
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "profile_pictures");

                // Kiểm tra xem thư mục có tồn tại không, nếu không thì tạo nó
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                // Tạo tên tệp ảnh mới
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(pictureFile.FileName);

                // Đường dẫn đầy đủ của ảnh
                var filePath = Path.Combine(uploadPath, fileName);

                // Lưu ảnh vào thư mục
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await pictureFile.CopyToAsync(stream);
                }

                // Lưu đường dẫn của ảnh vào trong Profile
                profile.Picture = "/uploads/profile_pictures/" + fileName;
            }
            else
            {
                // Nếu không upload ảnh mới, giữ nguyên ảnh cũ từ cơ sở dữ liệu
                var existingProfile = await _profileService.GetProfileByUserIdAsync(user.Id);
                if (existingProfile != null)
                {
                    profile.Picture = existingProfile.Picture; // Giữ nguyên đường dẫn ảnh cũ
                }
            }

            // Cập nhật profile trong cơ sở dữ liệu
            var result = await _profileService.UpdateProfileAsync(profile);
            if (result)
            {
                TempData["Success"] = "Profile updated successfully!";
                return RedirectToAction(nameof(Index));  // Chuyển hướng về trang index nếu cập nhật thành công
            }

            ModelState.AddModelError("", "An error occurred while updating the profile.");
            return View(profile); // Trả về View với thông tin hiện tại nếu lỗi
        }


        // Chức năng thay đổi mật khẩu
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound();
                }

                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    // Đăng xuất người dùng và chuyển hướng về trang đăng nhập
                    await _signInManager.SignOutAsync();
                    TempData["ErrorMessage"] = "Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.";
                    return RedirectToAction("Login", "Account"); // Hoặc thay đổi URL theo ý bạn
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
    }
}
