using Doctors_Web_Forum.BLL.IServices;
using Doctors_Web_Forum.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Doctors_Web_Forum.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class TopicController : Controller
    {
        private readonly ITopicService _topicService;

        public TopicController(ITopicService topicService)
        {
            _topicService = topicService;
        }

        // GET : Topics
        public async Task<IActionResult> Index(int pg = 1 , int pageSize = 5 , string searchTerm = "" )
        {
            var (topics, pager) = await _topicService.GetAllTopicsAsync(pg, pageSize, searchTerm);

            
            ViewBag.Pager = pager;

            
            ViewBag.SearchTerm = searchTerm;

            
            return View(topics);
        }

        // GET: Topics/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var topic = await _topicService.GetTopicByIdAsync(id);
            if (topic == null)
            {
                return NotFound();
            }
            return View(topic);
        }

        // GET: Topics/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Topics/Create
        [HttpPost]
        
        public async Task<IActionResult> Create([Bind("Id,TopicName,Description,Status")] Topic topic)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    await _topicService.AddTopicAsync(topic);

                    
                    TempData["success"] = "Create Topic Successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    
                    TempData["error"] = $"An error occurred while adding the topic! : {ex.Message}";
                    return View(topic);
                }
            }

            
            TempData["error"] = "Invalid topic information!";
            return View(topic);
        }


        // GET: Topics/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var topic = await _topicService.GetTopicByIdAsync(id);
            if (topic == null)
            {
                return NotFound();
            }
            return View(topic);
        }

        // POST: Topics/Edit/5
        [HttpPost]
        
        public async Task<IActionResult> Edit(int id, [Bind("Id,TopicName,Description,Status")] Topic topic)
        {
            if (id != topic.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update the topic
                    await _topicService.UpdateTopicAsync(topic);

                    // Success message
                    TempData["success"] = "Update Topic Successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (KeyNotFoundException)
                {
                    // Handle missing key
                    return NotFound();
                }
                catch (Exception ex)
                {
                    // Error message on failure
                    ModelState.AddModelError("", $"Error updating topic: {ex.Message}");
                    TempData["error"] = $"An error occurred while updating the topic: {ex.Message}";
                }
            }

            // Invalid model state
            TempData["error"] = "Invalid topic information!";
            return View(topic);
        }


        public async Task<IActionResult> Delete(int id)
        {
            try
            {

                var topic = await _topicService.GetTopicByIdAsync(id);
                if (topic == null)
                {
                    TempData["error"] = "Topic does not exist or has been deleted!";
                    return RedirectToAction("Index");
                }

                // Xóa chủ đề
                var isDeleted = await _topicService.DeleteTopicAsync(id);
                if (!isDeleted)
                {
                    TempData["error"] = "An error occurred while deleting the topic!";
                    return RedirectToAction("Index");
                }

                
                TempData["success"] = "Remove Topic Successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Ghi log lỗi nếu cần thiết
                TempData["error"] = $"An unexpected error occurred! : {ex.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
