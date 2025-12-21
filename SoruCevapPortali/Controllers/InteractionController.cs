using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoruCevapPortali.Data;
using SoruCevapPortali.Models;
using System.Security.Claims;

namespace SoruCevapPortali.Controllers
{
    [Authorize] // Sadece giriş yapmış kullanıcılar beğenebilir
    public class InteractionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InteractionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // KULLANICI ID'SİNİ ALMA METODU
        private int GetCurrentUserId()
        {
            // Not: Giriş sistemine göre burası değişebilir. 
            // Eğer User.Identity.Name kullanıyorsan oradan UserId bulmalıyız.
            // Şimdilik e-posta veya isimden user'ı bulduğumuzu varsayıyorum.
            var userName = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.User_name == userName || u.Email == userName);
            return user != null ? user.Id : 0;
        }

        // 1. SORUYU FAVORİYE EKLE / ÇIKAR
        [HttpPost]
        public IActionResult ToggleFavorite(int questionId)
        {
            int userId = GetCurrentUserId();
            if (userId == 0) return Json(new { success = false, message = "Kullanıcı bulunamadı." });

            var existingFav = _context.Favorites.FirstOrDefault(f => f.QuestionId == questionId && f.UserId == userId);
            bool isFavorited = false;

            if (existingFav != null)
            {
                _context.Favorites.Remove(existingFav); // Zaten ekliyse çıkar
                isFavorited = false;
            }
            else
            {
                var fav = new Favorite { QuestionId = questionId, UserId = userId };
                _context.Favorites.Add(fav); // Yoksa ekle
                isFavorited = true;
            }

            _context.SaveChanges();
            return Json(new { success = true, isFavorited = isFavorited });
        }

        // 2. CEVABI BEĞEN / BEĞENİ GERİ AL
        [HttpPost]
        public IActionResult ToggleAnswerLike(int answerId)
        {
            int userId = GetCurrentUserId();
            if (userId == 0) return Json(new { success = false, message = "Kullanıcı bulunamadı." });

            var existingLike = _context.AnswerLikes.FirstOrDefault(l => l.AnswerId == answerId && l.UserId == userId);
            bool isLiked = false;

            if (existingLike != null)
            {
                _context.AnswerLikes.Remove(existingLike);
                isLiked = false;
            }
            else
            {
                var like = new AnswerLike { AnswerId = answerId, UserId = userId };
                _context.AnswerLikes.Add(like);
                isLiked = true;
            }

            _context.SaveChanges();

            // Güncel beğeni sayısını da döndür
            int newCount = _context.AnswerLikes.Count(l => l.AnswerId == answerId);

            return Json(new { success = true, isLiked = isLiked, newCount = newCount });
        }
    }
}