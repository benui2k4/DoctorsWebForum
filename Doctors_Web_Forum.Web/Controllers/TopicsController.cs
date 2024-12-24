using Microsoft.AspNetCore.Mvc;

namespace Doctors_Web_Forum.Web.Controllers
{
    public class TopicsController: Controller
    {
        public ActionResult Index()
        {
            return View("~/Views/Topics/Topics.cshtml");
        }

    }
}
