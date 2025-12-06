using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoruCevapPortali.Data;
using SoruCevapPortali.Interfaces;
using SoruCevapPortali.Models;

namespace SoruCevapPortali.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AnswerController : Controller
    {
        private readonly IRepository<Answer> _answerRepository;
        private readonly ApplicationDbContext _context;

        public AnswerController(IRepository<Answer> answerRepository, ApplicationDbContext context)
        {
            _answerRepository = answerRepository;
            _context = context;
        }

        public IActionResult Index()
        {
            var answers = _context.Answers
                            .Include(c => c.User)
                            .Include(c => c.Question)
                            .ToList();
            return View(answers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var answer = _answerRepository.GetById(id);
            if (answer != null)
            {
                _answerRepository.Delete(answer);
            }
            return RedirectToAction(nameof(Index));
        }

        // --- İŞTE KRİTİK METOT: ToggleBestAnswer ---
        [HttpPost]
        public IActionResult ToggleBestAnswer(int id)
        {
            // Tablo adı 'Answers' olmalı (S takısına dikkat!)
            var answer = _context.Answers.Find(id);
            if (answer == null) return NotFound();

            // Durumu tersine çevir
            bool wasBest = answer.IsBestAnswer; // Modelde adı 'IsBestAnswer' olmalı
            answer.IsBestAnswer = !wasBest;

            // Eğer "En İyi" olarak işaretleniyorsa, diğerlerinin işaretini kaldır
            if (answer.IsBestAnswer)
            {
                var otherAnswers = _context.Answers
                    .Where(c => c.QuestionId == answer.QuestionId && c.Id != answer.Id)
                    .ToList();

                foreach (var other in otherAnswers)
                {
                    other.IsBestAnswer = false;
                }
            }

            _context.SaveChanges();

            // Güncellenmiş listeyi geri döndür
            var allAnswers = _context.Answers
                .Where(c => c.QuestionId == answer.QuestionId)
                .Select(c => new { id = c.Id, isBest = c.IsBestAnswer })
                .ToList();

            return Json(new { success = true, updatedAnswers = allAnswers });
        }
    }
}