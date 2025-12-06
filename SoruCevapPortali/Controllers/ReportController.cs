using Microsoft.AspNetCore.Mvc;
using SoruCevapPortali.Interfaces;
using SoruCevapPortali.Models;
using System.Security.Claims; // Kullanıcı ID'sini almak için

namespace SoruCevapPortali.Controllers
{
    public class ReportController : Controller
    {
        private readonly IRepository<Report> _reportRepository;
        private readonly IRepository<User> _userRepository;

        public ReportController(IRepository<Report> reportRepository, IRepository<User> userRepository)
        {
            _reportRepository = reportRepository;
            _userRepository = userRepository;
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Güvenlik için şart
        public IActionResult Create(int? questionId, int? answerId, string reason)
        {
            // Kullanıcı giriş yapmış mı?
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { success = false, message = "Şikayet etmek için giriş yapmalısınız." });
            }

            // Giriş yapan kullanıcının ID'sini bul
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return Json(new { success = false, message = "Kullanıcı bulunamadı." });

            int reporterId = int.Parse(userIdStr);

            var report = new Report
            {
                UserId = reporterId, // ReporterId DEĞİL, UserId
                reason = reason,     // Reason DEĞİL, reason
                QuestionId = questionId,
                AnswerId = answerId,
                creation_date = DateTime.Now, // CreatedAt DEĞİL
                is_resolved = false           // IsResolved DEĞİL
            };
            _reportRepository.Add(report);

            return Json(new { success = true, message = "Şikayetiniz alındı. Teşekkürler." });
        }
    }
}