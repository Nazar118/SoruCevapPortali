using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoruCevapPortali.Data;
using SoruCevapPortali.Models;
using System.Security.Claims;

namespace SoruCevapPortali.Controllers
{
    [Authorize] // Sadece giriş yapanlar görebilir
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Yardımcı Metot: Giriş yapan kullanıcının ID'sini bulur
        private int GetCurrentUserId()
        {
            var userName = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.User_name == userName || u.Email == userName);
            return user != null ? user.Id : 0;
        }

        // 1. FAVORİLERİM SAYFASI
        public IActionResult Favorites()
        {
            int userId = GetCurrentUserId();

            // Kullanıcının favorilediği soruları çekelim
            var favoriteQuestions = _context.Favorites
                .Include(f => f.Question)
                    .ThenInclude(q => q.Category)
                .Include(f => f.Question)
                    .ThenInclude(q => q.User)
                .Include(f => f.Question)
                    .ThenInclude(q => q.Answers)
                .Where(f => f.UserId == userId)
                .Select(f => f.Question) // Bize Favorite tablosu değil, içindeki Question lazım
                .OrderByDescending(q => q.creation_date)
                .ToList();

            // 1. FAVORİLERİM SAYFASI (GÜNCELLENMİŞ HALİ)
            var viewModels = favoriteQuestions.Select(q => new QuestionListViewModel
            {
                Id = q.Id,
                Title = q.title,
                ContentSummary = q.contents.Length > 100 ? q.contents.Substring(0, 100) + "..." : q.contents,

                // Eğer kategori silinmişse null hatası verme, "Kategorisiz" yaz
                CategoryName = q.Category != null ? q.Category.Name : "Kategorisiz",
                CategoryId = q.Category != null ? q.Category.Id : 0,

                // Eğer kullanıcı silinmişse hata verme
                UserName = q.User != null ? q.User.User_name : "Silinmiş Kullanıcı",

                AnswerCount = q.Answers.Count(a => !a.IsDeleted),
                CreatedDate = q.creation_date,
                IsSolved = q.Answers.Any(a => a.IsBestAnswer)
            }).ToList();

            return View(viewModels);
        }

        // 2. BEĞENDİKLERİM (CEVAPLAR) SAYFASI
        public IActionResult Likes()
        {
            int userId = GetCurrentUserId();

            // Kullanıcının beğendiği cevapları (AnswerLikes tablosundan) çekelim
            var likedAnswers = _context.AnswerLikes
                .Include(l => l.Answer)
                    .ThenInclude(a => a.Question) // Cevabın hangi soruya ait olduğunu bilmemiz lazım
                .Include(l => l.Answer)
                    .ThenInclude(a => a.User)     // Cevabı kimin yazdığını da görelim
                .Where(l => l.UserId == userId)
                .Select(l => l.Answer)        // Bize 'Like' tablosu değil, 'Answer'ın kendisi lazım
                .OrderByDescending(a => a.creation_date)
                .ToList();

            return View(likedAnswers);
        }
        // 3. SORULARIM SAYFASI
        public IActionResult MyQuestions()
        {
            int userId = GetCurrentUserId();

            // Sadece benim sorduğum (ve silinmemiş) soruları getir
            var myQuestions = _context.Questions
                .Include(q => q.Category)
                .Include(q => q.User)
                .Include(q => q.Answers)
                .Where(q => q.UserId == userId && !q.IsDeleted)
                .OrderByDescending(q => q.creation_date)
                .Select(q => new QuestionListViewModel
                {
                    Id = q.Id,
                    Title = q.title,
                    ContentSummary = q.contents.Length > 100 ? q.contents.Substring(0, 100) + "..." : q.contents,
                    CategoryName = q.Category.Name,
                    CategoryId = q.Category.Id,
                    UserName = q.User.User_name,
                    AnswerCount = q.Answers.Count(a => !a.IsDeleted),
                    CreatedDate = q.creation_date,
                    IsSolved = q.Answers.Any(a => a.IsBestAnswer),
                    // Kendi sorumuz olduğu için onay durumunu da görelim
                    StatusText = q.Is_it_approved ? "Yayında" : "Onay Bekliyor",
                    StatusClass = q.Is_it_approved ? "badge-success" : "badge-warning",
                    StatusIcon = q.Is_it_approved ? "fas fa-check" : "fas fa-clock"
                })
                .ToList();

            return View(myQuestions);
        }
    }
}