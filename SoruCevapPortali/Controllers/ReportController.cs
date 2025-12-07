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
            // 1. GÜVENLİK KONTROLÜ: Kullanıcı giriş yapmış mı?
            if (!User.Identity.IsAuthenticated)
            {
                // 401 (Yetkisiz) Hatası döndürür
                return Unauthorized(new { success = false, message = "Şikayet etmek için giriş yapmalısınız." });
            }

            // 2. KULLANICI ID ALMA (Daha Güvenli Yöntem)
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");

            // int.Parse yerine int.TryParse kullanıyoruz ki hata olursa çökmesin
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int reporterId))
            {
                return BadRequest(new { success = false, message = "Kullanıcı bilgisi alınamadı. Lütfen çıkış yapıp tekrar girin." });
            }

            // 3. MODEL OLUŞTURMA (Senin SQL İsimlerine Göre)
            var report = new Report
            {
                UserId = reporterId,          // SQL: UserId
                reason = reason,              // SQL: reason
                QuestionId = questionId,
                AnswerId = answerId,
                creation_date = DateTime.Now, // SQL: creation_date
                is_resolved = false           // SQL: is_resolved
            };

            // 4. VERİTABANI KAYDI (Hata Yakalamalı - Try/Catch)
            try
            {
                _reportRepository.Add(report);
                return Json(new { success = true, message = "Şikayetiniz alındı. Teşekkürler." });
            }
            catch (Exception ex)
            {
                // Hata oluşursa sunucuyu çökertme, hatayı yakala ve mesaj olarak dön
                return StatusCode(500, new { success = false, message = "Kaydedilirken bir hata oluştu." });
            }
        }
    }
}