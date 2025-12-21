using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SoruCevapPortali.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SoruCevapPortali.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // 1. KART VERİLERİ
            ViewData["TotalQuestions"] = _context.Questions.Count();
            ViewData["TotalUsers"] = _context.Users.Count();
            ViewData["TotalAnswers"] = _context.Answers.Count();
            ViewData["PendingQuestionsCount"] = _context.Questions.Count(q => !q.Is_it_approved);

            // 2. PASTA GRAFİK (Kategoriler)
            var categoryData = _context.Categories
                .Select(c => new { Name = c.Name, Count = c.Questions.Count() })
                .ToList();

            ViewBag.CatLabels = categoryData.Select(x => x.Name).ToArray();
            ViewBag.CatCounts = categoryData.Select(x => x.Count).ToArray();

            // 3. ÇİZGİ GRAFİK (Haftalık Kayıtlar)
            var last7Days = Enumerable.Range(0, 7)
                .Select(i => DateTime.Now.Date.AddDays(-i))
                .OrderBy(d => d)
                .ToList();

            var recentUsers = _context.Users
                .Where(u => u.registration_date >= DateTime.Now.Date.AddDays(-7))
                .ToList();

            var userCounts = last7Days.Select(date =>
                recentUsers.Count(u => u.registration_date.Date == date)
            ).ToArray();

            ViewBag.UserDates = last7Days.Select(d => d.ToString("dd MMM")).ToArray();
            ViewBag.UserCounts = userCounts;

            // --- 4. LİDERLİK TABLOSU VERİLERİ (GÜNCELLENDİ) ---

            // En Çok Soru Soran 5 Kişi (Sadece Silinmemiş ve Onaylanmışlar)
            var topQuestioners = _context.Users
                .Select(u => new {
                    Name = u.User_name,
                    Count = u.Questions.Count(q => !q.IsDeleted && q.Is_it_approved)
                })
                .Where(x => x.Count > 0) // Hiç sorusu olmayanları listeye alma
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToList();

            ViewBag.TopQuestionersNames = topQuestioners.Select(x => x.Name).ToList();
            ViewBag.TopQuestionersCounts = topQuestioners.Select(x => x.Count).ToList();


            // En Çok Cevap Veren 5 Kişi (Sadece Silinmemişler)
            var topAnswerers = _context.Users
                .Select(u => new {
                    Name = u.User_name,
                    // DÜZELTME: Sadece silinmemiş cevapları say
                    Count = u.Answers.Count(a => !a.IsDeleted)
                })
                .Where(x => x.Count > 0) // Hiç cevabı olmayanları listeye alma
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToList();

            ViewBag.TopAnswerersNames = topAnswerers.Select(x => x.Name).ToList();
            ViewBag.TopAnswerersCounts = topAnswerers.Select(x => x.Count).ToList();

            return View();
        }
    }
}