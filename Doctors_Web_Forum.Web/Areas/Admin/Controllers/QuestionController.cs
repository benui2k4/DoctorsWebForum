using Doctors_Web_Forum.BLL.IServices;
using Doctors_Web_Forum.DAL.Models;
using Doctors_Web_Forum.DAL.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Doctors_Web_Forum.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuestionController : Controller
    {
        private readonly IQuestionService _questionService;
        private readonly ITopicService _topicService;
        private readonly UserManager<User> _userManager;
        private readonly IAnswerService _answerService;

        public QuestionController(IQuestionService questionService, ITopicService topicService, UserManager<User> userManager, IAnswerService answerService)
        {
            _questionService = questionService;
            _topicService = topicService;
            _userManager = userManager;
            _answerService = answerService;
        }

        // GET: Admin/Question/Index
        public async Task<IActionResult> Index(int pg = 1, int pageSize = 5, string searchTerm = "")
        {
            var (questions, pager) = await _questionService.GetAllQuestionsAsync(pg, pageSize, searchTerm);
            ViewBag.Pager = pager;
            ViewBag.SearchTerm = searchTerm;
            return View(questions);
        }

        public async Task<IActionResult> Details(int id)
        {
            var question = await _questionService.GetQuestionByIdAsync(id);
            if (question == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy câu hỏi.";
                return RedirectToAction(nameof(Index));
            }

            var user = await _userManager.FindByIdAsync(question.UserId);
            ViewBag.UserName = user?.UserName ?? "Người dùng không tồn tại";
            ViewBag.UserEmail = user?.Email ?? "Email không có sẵn";

            var answers = await _answerService.GetAnswersByQuestionIdAsync(id);
            var currentUser = await _userManager.GetUserAsync(User);
            ViewBag.CurrentUserId = currentUser?.Id;

            ViewBag.Answers = answers.Select(a => new AnswerViewModel
            {
                Id = a.Id,
                UserId = a.UserId,
                UserName = a.User.UserName,
                UserEmail = a.User.Email, // Lấy email của người trả lời
                AnswerText = a.AnswerText,
                PostedDate = a.PostedDate,
                Status = a.Status,
                IsCurrentUser = currentUser?.Id == a.UserId
            }).ToList();

            return View(question);
        }




        // GET: Admin/Question/Create
        public async Task<IActionResult> Create()
        {
            await PopulateTopicsViewBag();
            return View();
        }

        // POST: Admin/Question/Create
        [HttpPost]
        public async Task<IActionResult> Create([Bind("UserId, TopicId, QuestionText, Description")] Question question)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                question.UserId = user.Id;
            }

            await _questionService.CreateQuestionAsync(question);
            TempData["SuccessMessage"] = "Câu hỏi đã được tạo thành công!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Question/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var question = await _questionService.GetQuestionByIdAsync(id);
            if (question == null)
            {
                TempData["ErrorMessage"] = "Câu hỏi không được tìm thấy để chỉnh sửa.";
                return RedirectToAction(nameof(Index));
            }

            await PopulateTopicsViewBag();
            return View(question);
        }

        // POST: Admin/Question/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id, UserId, TopicId, QuestionText, Description, Status")] Question question)
        {
            if (id != question.Id)
            {
                TempData["ErrorMessage"] = "Dữ liệu không hợp lệ!";
                return RedirectToAction(nameof(Index));
            }

            var updatedQuestion = await _questionService.UpdateQuestionAsync(id, question.QuestionText, question.Description, question.TopicId);
            if (updatedQuestion == null)
            {
                TempData["ErrorMessage"] = "Câu hỏi này không tìm thấy để cập nhật.";
                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] = "Câu hỏi đã được cập nhật thành công!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Question/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _questionService.DeleteQuestionAsync(id);
            if (!result)
            {
                TempData["ErrorMessage"] = "Không thể xóa câu hỏi! Câu hỏi không tồn tại hoặc đã xảy ra lỗi.";
                return RedirectToAction("Index");
            }

            TempData["SuccessMessage"] = "Câu hỏi đã được xóa thành công!";
            return RedirectToAction("Index");
        }

        private async Task PopulateTopicsViewBag()
        {
            const int pageNumber = 1;
            const int pageSize = 5;
            var (topics, pager) = await _topicService.GetAllTopicsAsync(pageNumber, pageSize, string.Empty);
            ViewBag.Topics = topics;  // Lấy danh sách chủ đề
        }


        // POST: Admin/Question/AddAnswer
        [HttpPost]
        public async Task<IActionResult> AddAnswer(int questionId, [Bind("AnswerText")] Answer answer)
        {
            
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    answer.UserId = user.Id; // Gán UserId từ người dùng đã đăng nhập
                    answer.QuestionId = questionId; // Gán QuestionId

                    await _answerService.CreateAnswerAsync(answer);
                    TempData["SuccessMessage"] = "Câu trả lời đã được thêm thành công!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Người dùng hiện tại không được tìm thấy!";
                }
           

            return RedirectToAction("Details", new { id = questionId });
        }


        [HttpGet]
        public async Task<IActionResult> EditAnswer(int id)
        {
            var answer = await _answerService.GetAnswerByIdAsync(id);
            if (answer == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy câu trả lời.";
                return RedirectToAction("Details", new { id = answer.QuestionId });
            }

            // Kiểm tra quyền: Người dùng hiện tại phải là người trả lời
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || currentUser.Id != answer.UserId)
            {
                TempData["ErrorMessage"] = "Bạn không có quyền sửa câu trả lời này.";
                return RedirectToAction("Details", new { id = answer.QuestionId });
            }

            // Chuyển đổi sang AnswerViewModel
            var answerViewModel = new AnswerViewModel
            {
                Id = answer.Id,
                QuestionId = answer.QuestionId,
                AnswerText = answer.AnswerText,
                UserId = answer.UserId
            };

            return View(answerViewModel);
        }

        [HttpPost]
        
        public async Task<IActionResult> EditAnswer(AnswerViewModel model)
        {
           

            var answer = await _answerService.GetAnswerByIdAsync(model.Id);
            if (answer == null)
            {
                TempData["ErrorMessage"] = "Câu trả lời không tồn tại.";
                return RedirectToAction("Details", new { id = model.QuestionId });
            }

            // Kiểm tra quyền
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || currentUser.Id != answer.UserId)
            {
                TempData["ErrorMessage"] = "Bạn không có quyền sửa câu trả lời này.";
                return RedirectToAction("Details", new { id = model.QuestionId });
            }

            // Cập nhật nội dung câu trả lời
            answer.AnswerText = model.AnswerText;
            await _answerService.UpdateAnswerAsync(answer.Id, answer.AnswerText);

            TempData["SuccessMessage"] = "Câu trả lời đã được cập nhật thành công.";
            return RedirectToAction("Details", new { id = model.QuestionId });
        }


        [HttpGet] // Đảm bảo sử dụng GET ở đây
        public async Task<IActionResult> DeleteAnswer(int id)
        {
            var answer = await _answerService.GetAnswerByIdAsync(id);
            if (answer == null)
            {
                TempData["ErrorMessage"] = "Câu trả lời không tồn tại.";
                return RedirectToAction("Details", new { id = 0 }); // Redirect về trang hợp lý
            }

            // Kiểm tra quyền: Chỉ người trả lời mới được phép xóa
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || currentUser.Id != answer.UserId)
            {
                TempData["ErrorMessage"] = "Bạn không có quyền xóa câu trả lời này.";
                return RedirectToAction("Details", new { id = answer.QuestionId });
            }

            // Xóa câu trả lời
            var result = await _answerService.DeleteAnswerAsync(id);
            if (!result)
            {
                TempData["ErrorMessage"] = "Xóa câu trả lời thất bại.";
            }
            else
            {
                TempData["SuccessMessage"] = "Câu trả lời đã được xóa thành công.";
            }

            return RedirectToAction("Details", new { id = answer.QuestionId });
        }




    }
}
