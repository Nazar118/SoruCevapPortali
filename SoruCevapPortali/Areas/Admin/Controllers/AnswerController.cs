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

       

        // 1. Düzenleme Sayfasını Aç (GET)
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var answer = _answerRepository.GetById(id);
            if (answer == null)
            {
                return NotFound();
            }

            // Hangi soruya ait olduğunu View'da göstermek için Viewbag'e atalım
            ViewBag.QuestionId = answer.QuestionId;

            return View(answer);
        }

        // 2. Düzenlemeyi Kaydet (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Answer answer)
        {
            if (ModelState.IsValid)
            {
                // Veritabanından orijinal kaydı çekiyoruz
                var existingAnswer = _answerRepository.GetById(answer.Id);

                if (existingAnswer == null)
                {
                    return NotFound();
                }

                // Sadece değişmesine izin verdiğimiz alanları güncelliyoruz.
                // Böylece UserId, QuestionId, creation_date gibi alanlar bozulmaz.
                existingAnswer.contents = answer.contents;
                existingAnswer.IsBestAnswer = answer.IsBestAnswer;

                _answerRepository.Update(existingAnswer);

                return RedirectToAction(nameof(Index));
            }
            return View(answer);
        }
        // ==========================================================


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

            bool wasBest = answer.IsBestAnswer;
            answer.IsBestAnswer = !wasBest;

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

            var allAnswers = _context.Answers
                .Where(c => c.QuestionId == answer.QuestionId)
                .Select(c => new { id = c.Id, isBest = c.IsBestAnswer })
                .ToList();

            return Json(new { success = true, updatedAnswers = allAnswers });
        }
    }
}