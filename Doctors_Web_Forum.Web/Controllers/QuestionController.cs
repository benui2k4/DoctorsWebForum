using Microsoft.AspNetCore.Mvc;
using Doctors_Web_Forum.Services; // Namespace của các dịch vụ (nếu bạn có sử dụng)
using Doctors_Web_Forum.Models; // Namespace chứa các mô hình (Models)
using System.Threading.Tasks;
using Doctors_Web_Forum.BLL.IServices;
using Microsoft.EntityFrameworkCore;
using Doctors_Web_Forum.DAL.Models.ViewModel;
using Doctors_Web_Forum.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Doctors_Web_Forum.BLL.Services;
using System.Security.Claims;

namespace Doctors_Web_Forum.Web.Controllers
{
    public class QuestionController : Controller
    {
        private readonly IQuestionService _questionService;
        private readonly IAnswerService _answerService;
        private readonly UserManager<User> _userManager;

        public QuestionController(IQuestionService questionService, IAnswerService answerService, UserManager<User> userManager)
        {
            _questionService = questionService;
            _answerService = answerService;
            _userManager = userManager;
        }

        // GET: /Question/Index
        public async Task<IActionResult> Index(int? topicId, int pg = 1, int pageSize = 5, string searchTerm = "")
        {
            IEnumerable<Question> questions;
            Paginate pager;

            if (topicId.HasValue && topicId > 0) // Kiểm tra nếu topicId được truyền
            {
                // Lọc câu hỏi theo topicId
                var (questionsByTopic, pagerByTopic) = await _questionService.GetQuestionsByTopicAsync(topicId.Value, pg, pageSize);
                questions = questionsByTopic;
                pager = pagerByTopic;

                ViewBag.TopicId = topicId; // Để hiển thị chủ đề hiện tại trong View
            }
            else
            {
                // Hiển thị tất cả câu hỏi nếu không có topicId
                var (allQuestions, allPager) = await _questionService.GetAllQuestionsAsync(pg, pageSize, searchTerm);
                questions = allQuestions;
                pager = allPager;
            }

            ViewBag.Pager = pager;
            ViewBag.SearchTerm = searchTerm;

            return View(questions);
        }


        // GET: /Question/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            // Lấy thông tin câu hỏi từ id
            var question = await _questionService.GetQuestionByIdAsync(id);
            if (question == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy câu hỏi.";
                return RedirectToAction(nameof(Index));
            }

            // Lấy danh sách câu trả lời liên quan đến câu hỏi
            var answers = question.Answers.Select(a => new AnswerViewModel
            {
                Id = a.Id,
                UserName = a.User.UserName,
                AnswerText = a.AnswerText,
                PostedDate = a.PostedDate
            }).ToList();

            // Truyền dữ liệu vào View
            ViewBag.Answers = answers;

            return View(question); // Truyền câu hỏi vào View
        }

        [HttpPost]
        public async Task<IActionResult> Create(Question model)
        {
            // Kiểm tra người dùng đã đăng nhập
            if (!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "Bạn cần đăng nhập để thêm câu hỏi.";
                return Redirect("/Login");
            }

            // Gán thông tin cho câu hỏi
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            model.UserId = userId;
            model.PostDate = DateTime.Now;
            model.Status = true; // Giả định trạng thái là Active khi thêm câu hỏi

            // Thêm câu hỏi qua dịch vụ
            var result = await _questionService.AddQuestionAsync(model);

            if (!result)
            {
                TempData["ErrorMessage"] = "Thêm câu hỏi thất bại. Vui lòng thử lại.";
                return RedirectToAction("Index");
            }

            TempData["SuccessMessage"] = "Thêm câu hỏi thành công.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddAnswer(int questionId, string answerText)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "You need to log in to add an answer.";
                return Redirect("http://localhost:5140/Login"); // URL tuyệt đối đến trang Login
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(answerText) || string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "Answer cannot be empty.";
                return RedirectToAction("Details", new { id = questionId });
            }

            var newAnswer = new Answer
            {
                QuestionId = questionId,
                UserId = userId,
                AnswerText = answerText,
                PostedDate = DateTime.Now,
                Status = true
            };

            var result = await _answerService.AddAnswerAsync(newAnswer);

            if (!result)
            {
                TempData["ErrorMessage"] = "Failed to add the answer. Please try again.";
                return RedirectToAction("Details", new { id = questionId });
            }

            TempData["SuccessMessage"] = "Your answer has been submitted successfully.";
            return RedirectToAction("Details", new { id = questionId });
        }


    }

}