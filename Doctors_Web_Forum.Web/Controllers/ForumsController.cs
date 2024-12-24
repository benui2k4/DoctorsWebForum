using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Doctors_Web_Forum.Web.Controllers
{
    public class ForumsController : Controller
    {
        // GET: ForumsController
        public ActionResult Index()
        {
            var categories = new List<dynamic>
        {
            new { Id = 1, Name = "Diabetes", Description = "Discuss and get support for diabetes management.", LastPost = "What is the best diet for Type 2?" },
            new { Id = 2, Name = "Heart Disease", Description = "Support and information on heart-related health issues.", LastPost = "Managing blood pressure at home." },
            new { Id = 3, Name = "Mental Health", Description = "Share and get advice on mental health.", LastPost = "How do you manage stress effectively?" }
        };

            // Mock data for recent posts
            var recentPosts = new List<dynamic>
        {
            new { Id = 101, QuestionText = "How do you manage stress effectively?", TopicName = "Mental Health", UserName = "User1", PostDate = "12/15/2024" },
            new { Id = 102, QuestionText = "Top exercises for better heart health", TopicName = "Heart Disease", UserName = "User2", PostDate = "12/14/2024" },
            new { Id = 103, QuestionText = "What is a healthy breakfast for diabetics?", TopicName = "Diabetes", UserName = "User3", PostDate = "12/13/2024" }
        };

            // Pass mock data to the view
            ViewBag.Categories = categories;
            ViewBag.RecentPosts = recentPosts;

            return View("~/Views/Forums/Forums.cshtml");
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
