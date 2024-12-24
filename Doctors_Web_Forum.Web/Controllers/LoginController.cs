using Microsoft.AspNetCore.Mvc;

namespace Doctors_Web_Forum.Web.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Login/Login.cshtml");
        }
    }
}
