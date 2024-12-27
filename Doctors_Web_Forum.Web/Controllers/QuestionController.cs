using Microsoft.AspNetCore.Mvc;
using Doctors_Web_Forum.BLL.IServices;
using Doctors_Web_Forum.DAL.Models.ViewModel;
using Doctors_Web_Forum.DAL.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Index(int? topicId, int pg = 1, int pageSize = 5)
        {
            IEnumerable<Question> questions;
            Paginate pager;

            if (topicId.HasValue && topicId > 0)
            {
                // Filter questions by TopicId
                var (questionsByTopic, pagerByTopic) = await _questionService.GetQuestionsByTopicAsync(topicId.Value, pg, pageSize);
                questions = questionsByTopic;
                pager = pagerByTopic;

                ViewBag.TopicId = topicId;
            }
            else
            {
                // Display all questions if no TopicId is provided
                var (allQuestions, allPager) = await _questionService.GetAllQuestionsAsync(pg, pageSize, "");
                questions = allQuestions;
                pager = allPager;
            }

            ViewBag.Pager = pager;
            return View(questions);
        }

        // GET: /Question/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            // Retrieve question details by id
            var question = await _questionService.GetQuestionByIdAsync(id);
            if (question == null)
            {
                TempData["ErrorMessage"] = "The question could not be found.";
                return RedirectToAction(nameof(Index));
            }

            // Retrieve answers related to the question
            var answers = question.Answers.Select(a => new AnswerViewModel
            {
                Id = a.Id,
                UserName = a.User.UserName,
                AnswerText = a.AnswerText,
                PostedDate = a.PostedDate
            }).ToList();

            // Pass data to the view
            ViewBag.Answers = answers;

            return View(question);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Question model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "You need to log in to add a question.";
                return RedirectToAction("Index");
            }
            if (model.TopicId <= 0 || string.IsNullOrEmpty(model.QuestionText) || string.IsNullOrEmpty(model.Description))
            {
                TempData["ErrorMessage"] = "Invalid data.";
                return RedirectToAction("Index");
            }

            // Assign user details to the question
            model.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            model.PostDate = DateTime.Now;
            model.Status = true;

            var result = await _questionService.AddQuestionAsync(model);

            if (!result)
            {
                TempData["ErrorMessage"] = "Failed to add the question.";
                return RedirectToAction("Index", new { topicId = model.TopicId });
            }

            TempData["SuccessMessage"] = "Question added successfully.";
            return RedirectToAction("Index", new { topicId = model.TopicId });
        }

        [HttpPost]
        public async Task<IActionResult> AddAnswer(int questionId, string answerText)
        {
            var user = await _userManager.GetUserAsync(User);

            // Check if the user is logged in
            if (user == null)
            {
                TempData["ErrorMessage"] = "You need to log in to submit an answer. Please log in and try again.";
                return RedirectToAction("Details", new { id = questionId });
            }

            // Validate the content of the answer
            if (string.IsNullOrEmpty(answerText))
            {
                TempData["ErrorMessage"] = "The answer content cannot be empty. Please provide a valid answer.";
                return RedirectToAction("Details", new { id = questionId });
            }

            // Create a new answer
            var newAnswer = new Answer
            {
                QuestionId = questionId,
                AnswerText = answerText,
                UserId = user.Id,
                PostedDate = DateTime.Now,
                Status = true // Assuming the answer is active
            };

            // Save the answer using the service
            var result = await _answerService.CreateAnswerAsync(newAnswer);
            if (result == null)
            {
                TempData["ErrorMessage"] = "Failed to submit the answer. Please try again later.";
            }
            else
            {
                TempData["SuccessMessage"] = "Your answer has been submitted successfully!";
            }

            return RedirectToAction("Details", new { id = questionId });
        }
    }
}