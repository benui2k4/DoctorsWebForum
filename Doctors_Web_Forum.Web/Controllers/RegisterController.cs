using Microsoft.AspNetCore.Mvc;

namespace Doctors_Web_Forum.Web.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Register/Register.cshtml");
        }
    }
}
