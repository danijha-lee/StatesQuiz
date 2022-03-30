using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StatesQuiz.Data;
using StatesQuiz.Models;
using StatesQuiz.Models.ViewModel;
using StatesQuiz.Services.Interfaces;

namespace StatesQuiz.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly ISQQuizService _quizService;

        public HomeController(ILogger<HomeController> logger,
                                ApplicationDbContext context,
                                ISQQuizService quizService)
        {
            _logger = logger;
            _context = context;
            _quizService = quizService;
        }

        public IActionResult Login(int? attempt = 1)
        {
            if (attempt > 3)
            {
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                ViewData["Attempt"] = attempt;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string UserName, string Password, int attempt)
        {
            if (ModelState.IsValid)
            {
                if (UserName != null && Password != null)
                {
                    try
                    {
                        User result = await _quizService.SearchUserNameAsync(UserName);

                        if (result != null && UserName == result.UserName && Password == result.Password)
                        {
                            return RedirectToAction("PreQuiz", "Home");
                        }
                        else
                        {
                            ++attempt;
                            return RedirectToAction(nameof(Login), new { attempt = attempt });
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            return RedirectToAction(nameof(Login), new { attempt = attempt });
        }

        public async Task<IActionResult> PreviousResults(int Id)
        {
            if (Id != null)
            {
                int userId = Id;
                PreviousResultsViewModel model = new PreviousResultsViewModel();
                model.PreviousResults = await _quizService.GetPreviousTestResultsAsync(userId);
                return View(model);
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Index(int? attempt = 1)
        {
            if (attempt > 3)
            {
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                ViewData["Attempt"] = attempt;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string UserName, string Password, int attempt)
        {
            if (ModelState.IsValid)
            {
                if (UserName != null && Password != null)
                {
                    try
                    {
                        User result = await _quizService.SearchUserNameAsync(UserName);

                        if (result != null && UserName == result.UserName && Password == result.Password)
                        {
                            return RedirectToAction("PreQuiz", "Home");
                        }
                        else
                        {
                            ++attempt;
                            return RedirectToAction(nameof(Index), new { attempt = attempt });
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            return RedirectToAction(nameof(Index), new { attempt = attempt });
        }

        public IActionResult Lockout()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string UserName, string Password, string Email, string FirstName)
        {
            return View();
        }

        public IActionResult PreQuiz()
        {
            return View();
        }

        

        public async Task<IActionResult> Quiz()
        {
            QuizViewModel model = new QuizViewModel();
            model.States = await _quizService.GetQuizQuestionsAsync();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Quiz(States state_1, States state_2, States state_3, States state_4, States state_5, States state_6, States state_7, States state_8, States state_9, States state_10)
        {
            if (ModelState.IsValid)
            {
                List<States> answers = new List<States> { state_1, state_2, state_3, state_4, state_5, state_6, state_7, state_8, state_9, state_10 };
                int quizGrade = 0;

                foreach (var answer in answers)
                {
                    var value = await _quizService.CheckUserInput(answer);
                    if (value == true)
                    {
                        quizGrade += 10;
                    }
                }
                TestResult testResult = await _quizService.GradeTestAsync(quizGrade);
                return RedirectToAction("QuizResults", testResult);
            }
            QuizViewModel model = new QuizViewModel();
            return View(model);
        }

        public IActionResult QuizResults(TestResult testResult)
        {
            return View(testResult);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}