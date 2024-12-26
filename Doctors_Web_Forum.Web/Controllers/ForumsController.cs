using Doctors_Web_Forum.BLL.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Doctors_Web_Forum.Web.Controllers
{
    public class ForumsController : Controller
    {
        private readonly ITopicService _topicService;
        private readonly IQuestionService _questionService;
        // GET: ForumsController

        public ForumsController(ITopicService topicService, IQuestionService questionService)
        {
            _topicService = topicService;
            _questionService = questionService;
        }
        public async Task<ActionResult> Index(int pg = 1, int pageSize = 1000, string query = "")
        {
            // Lấy các chủ đề (categories) từ service
            var (categories, topicPager) = await _topicService.GetAllTopicsAsync(pg, pageSize, query);

            // Lấy các bài viết gần đây (recent posts)
            var (recentPosts, postPager) = await _questionService.GetRecentPostsAsync(pg, pageSize, query);

            // Truyền dữ liệu vào View
            ViewBag.Categories = categories; // Danh sách chủ đề
            ViewBag.RecentPosts = recentPosts; // Danh sách bài viết gần đây
            ViewBag.TopicPager = topicPager; // Phân trang chủ đề
            ViewBag.PostPager = postPager; // Phân trang bài viết gần đây
            ViewBag.SearchTerm = query; // Từ khóa tìm kiếm

            return View();
        }

        // GET: ForumsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ForumsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ForumsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ForumsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ForumsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ForumsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ForumsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}