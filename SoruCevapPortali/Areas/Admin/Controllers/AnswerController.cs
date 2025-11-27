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
            // İŞTE DÜZELTME BURADA!
            // Artık Türkçe isimleri değil, Adım 1'de yazdığımız İngilizce isimleri kullanıyoruz.
            var answers = _context.Answers
                            .Include(c => c.User)      // Eskiden: .Include(c => c.CevaplayanKullanici)
                            .Include(c => c.Question)  // Eskiden: .Include(c => c.AitOlduguSoru)
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

        [HttpPost]
        public IActionResult ToggleBestAnswer(int id)
        {
            var answer = _context.Answers.Find(id);
            if (answer == null) return NotFound();

            bool wasBest = answer.Is_it_the_best_answer;
            answer.Is_it_the_best_answer = !wasBest;

            if (answer.Is_it_the_best_answer)
            {
                // BURADA DA QuestionId kullanıyoruz
                var otherAnswers = _context.Answers
                    .Where(c => c.QuestionId == answer.QuestionId && c.Id != answer.Id)
                    .ToList();

                foreach (var other in otherAnswers)
                {
                    other.Is_it_the_best_answer = false;
                }
            }

            _context.SaveChanges();

            // BURADA DA QuestionId kullanıyoruz
            var allAnswers = _context.Answers
                .Where(c => c.QuestionId == answer.QuestionId)
                .Select(c => new { id = c.Id, isBest = c.Is_it_the_best_answer })
                .ToList();

            return Json(new { success = true, updatedAnswers = allAnswers });
        }
    }
}