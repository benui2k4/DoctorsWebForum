using Microsoft.AspNetCore.Mvc;

namespace Doctors_Web_Forum.Web.Controllers
{
    public class IntroductionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
