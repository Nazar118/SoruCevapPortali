using Microsoft.AspNetCore.Mvc;
using SoruCevapPortali.Data;
using SoruCevapPortali.Models;
using System.Security.Claims;

namespace SoruCevapPortali.Controllers
{
    public class AnswerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnswerController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int QuestionId, string contents)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // AuthController Admin area içindeyse:
                return RedirectToAction("Login", "Auth", new { area = "Admin" });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");

            if (userId != null && !string.IsNullOrEmpty(contents))
            {
                var answer = new Answer
                {
                    QuestionId = QuestionId,
                    UserId = int.Parse(userId),
                    contents = contents,
                    creation_date = DateTime.Now,
                    IsBestAnswer = false
                };

                _context.Answers.Add(answer);
                _context.SaveChanges();
            }

            // Cevap yazdıktan sonra tekrar sorunun detay sayfasına dön
            return RedirectToAction("Details", "Question", new { id = QuestionId });
        }
    }
}