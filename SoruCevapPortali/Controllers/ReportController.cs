using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoruCevapPortali.Data;
using SoruCevapPortali.Models;
using System.Security.Claims;

namespace SoruCevapPortali.Controllers
{
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Create(int? questionId, int? answerId, string reason)
        {
            // 1. GÜVENLİK KONTROLÜ: Kullanıcı giriş yapmış mı?
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { success = false, message = "Şikayet etmek için giriş yapmalısınız." });
            }

            // 2. KULLANICI ID ALMA
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");

            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int reporterId))
            {
                return Json(new { success = false, message = "Kullanıcı bilgisi alınamadı. Lütfen tekrar giriş yapın." });
            }

            if (string.IsNullOrEmpty(reason))
            {
                return Json(new { success = false, message = "Lütfen bir şikayet sebebi belirtin." });
            }

            // 3. MODEL OLUŞTURMA
            // NOT: Buradaki değişken isimlerinin (Reason, ReportDate vs.) Report.cs modelinle AYNI olması lazım.
            // Eğer modelinde küçük harf kullandıysan (reason, creation_date) buraları ona göre düzeltmelisin.
            var report = new Report
            {
                UserId = reporterId,
                reason = reason,                // Modelinde 'reason' ise burayı düzelt
                QuestionId = questionId,
                AnswerId = answerId,
                creation_date = DateTime.Now,      // Modelinde 'creation_date' ise burayı düzelt
                is_resolved = false              // Modelinde 'is_resolved' ise burayı düzelt
            };

            // 4. VERİTABANI KAYDI
            try
            {
                _context.Reports.Add(report);
                _context.SaveChanges();
                return Json(new { success = true, message = "Şikayetiniz başarıyla alındı. Teşekkürler." });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Bir hata oluştu. Lütfen daha sonra tekrar deneyin." });
            }
        }
    }
}