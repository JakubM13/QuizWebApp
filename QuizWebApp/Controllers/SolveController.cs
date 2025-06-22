using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.AspNetCore.Mvc;
using QuizWebApp.Models;
using QuizWebApp;
using Microsoft.AspNetCore.Authorization;


namespace QuizWebApp.Controllers
{
    [Authorize]
    public class SolveController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SolveController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int id)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);
            if (quiz == null) return NotFound();
            return View(quiz);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(int id, Dictionary<int, int> selectedAnswers)
        {
            var user = await _userManager.GetUserAsync(User);
            var quiz = await _context.Quizzes.Include(q => q.Questions).ThenInclude(q => q.Answers).FirstOrDefaultAsync(q => q.Id == id);

            if (quiz == null) return NotFound();

            int score = 0;
            var result = new UserQuizResult
            {
                QuizId = id,
                UserId = user.Id,
                CompletedAt = DateTime.UtcNow,
                Answers = new List<UserAnswer>()
            };

            foreach (var question in quiz.Questions)
            {
                if (selectedAnswers.TryGetValue(question.Id, out int answerId))
                {
                    var selected = question.Answers.FirstOrDefault(a => a.Id == answerId);
                    if (selected != null)
                    {
                        result.Answers.Add(new UserAnswer
                        {
                            QuestionId = question.Id,
                            SelectedAnswerId = selected.Id
                        });
                        if (selected.IsCorrect) score += question.Points;
                    }
                }
            }

            result.Score = score;
            _context.UserQuizResults.Add(result);
            await _context.SaveChangesAsync();
            return RedirectToAction("Result", new { id = result.Id });
        }

        public async Task<IActionResult> Result(int id)
        {
            var result = await _context.UserQuizResults.Include(r => r.Quiz).FirstOrDefaultAsync(r => r.Id == id);
            return View(result);
        }
    }
}
