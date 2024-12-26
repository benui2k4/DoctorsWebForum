using Doctors_Web_Forum.BLL.IServices;
using Doctors_Web_Forum.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Doctors_Web_Forum.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITopicService _topicService;

        public HomeController(ILogger<HomeController> logger, ITopicService topicService)
        {
            _logger = logger;
            _topicService = topicService;
        }

        public async Task<IActionResult> Index(int pg = 1, int pageSize = 5)
        {
            // Lấy danh sách topics từ service
            var (topics, paginate) = await _topicService.GetAllTopicsAsync(pg, pageSize, "");

            // Truyền dữ liệu (topics, paginate) sang View
            return View((topics, paginate));
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()    
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
