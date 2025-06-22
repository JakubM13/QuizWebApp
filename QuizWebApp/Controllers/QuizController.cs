using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.AspNetCore.Mvc;
using QuizWebApp.Models;
using QuizWebApp;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace QuizWebApp.Controllers
{
    [Authorize]
    public class QuizController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public QuizController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> MyQuizzes()
        {
            var user = await _userManager.GetUserAsync(User);
            var quizzes = await _context.Quizzes.Where(q => q.UserId == user.Id).ToListAsync();
            return View(quizzes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Quiz quiz)
        {
            var user = await _userManager.GetUserAsync(User);
            quiz.UserId = user.Id;
            _context.Quizzes.Add(quiz);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Quiz został pomyślnie utworzony!";
            return RedirectToAction("MyQuizzes");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var quiz = await _context.Quizzes.FirstOrDefaultAsync(q => q.Id == id && q.UserId == user.Id);
            if (quiz == null) return NotFound();
            return View(quiz);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Quiz quiz)
        {
            var user = await _userManager.GetUserAsync(User);
            var existingQuiz = await _context.Quizzes.FirstOrDefaultAsync(q => q.Id == quiz.Id && q.UserId == user.Id);
            if (existingQuiz == null) return NotFound();

            existingQuiz.Title = quiz.Title;
            existingQuiz.Description = quiz.Description;
            await _context.SaveChangesAsync();
            TempData["Success"] = "Quiz został zaktualizowany.";
            return RedirectToAction("MyQuizzes");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var quiz = await _context.Quizzes.FirstOrDefaultAsync(q => q.Id == id && q.UserId == user.Id);
            if (quiz == null) return NotFound();
            return View(quiz);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var quiz = await _context.Quizzes.FirstOrDefaultAsync(q => q.Id == id && q.UserId == user.Id);
            if (quiz == null) return NotFound();

            _context.Quizzes.Remove(quiz);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Quiz został usunięty.";
            return RedirectToAction("MyQuizzes");
        }

        public async Task<IActionResult> Stats(int id)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers)
                .Include(q => q.Results)
                    .ThenInclude(r => r.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quiz == null || quiz.CreatorId != _userManager.GetUserId(User))
                return NotFound();

            var stats = new
            {
                TotalCompleted = quiz.Results.Count,
                AverageScore = quiz.Results.Any() ? quiz.Results.Average(r => r.Score) : 0,
                Questions = quiz.Questions.Select(q => new
                {
                    q.Content,
                    Answers = q.Answers.Select(a => new
                    {
                        a.Content,
                        Percentage = quiz.Results.Count == 0 ? 0 :
                            100.0 * quiz.Results.SelectMany(r => r.Answers)
                                       .Count(ua => ua.QuestionId == q.Id && ua.SelectedAnswerId == a.Id)
                                    / quiz.Results.Count
                    })
                })
            };

            return View(stats);
        }
    }
}
