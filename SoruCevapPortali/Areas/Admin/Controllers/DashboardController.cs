using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SoruCevapPortali.Data;
using System.Linq;

namespace SoruCevapPortali.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // 1. KARTLAR İÇİN TEMEL İSTATİSTİKLER
            ViewData["TotalQuestions"] = _context.Questions.Count();
            ViewData["TotalUsers"] = _context.Users.Count();
            ViewData["PendingQuestions"] = _context.Questions.Count(q => !q.Is_it_approved); // Onaylanmamışlar

            // 2. GRAFİK 1: KATEGORİ DAĞILIMI (Pasta Grafik)
            // Kategorilere göre soru sayılarını grupla
            var categoryData = _context.Questions
                .GroupBy(q => q.Category.Name)
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .ToList();

            // Verileri iki ayrı diziye ayır (İsimler ve Sayılar) - Chart.js bu formatı sever
            ViewBag.CategoryLabels = categoryData.Select(x => x.Category).ToArray();
            ViewBag.CategoryCounts = categoryData.Select(x => x.Count).ToArray();


            // 3. GRAFİK 2: HAFTALIK KULLANICI KAYITLARI (Çizgi Grafik)
            // Son 7 günün tarihlerini al
            var last7Days = Enumerable.Range(0, 7)
                .Select(i => DateTime.Now.Date.AddDays(-i))
                .OrderBy(d => d) // Tarihe göre sırala (Eskiden yeniye)
                .ToList();

            // Veritabanından son 7 günde kayıt olanları çek
            var userSignups = _context.Users
                .Where(u => u.registration_date >= DateTime.Now.Date.AddDays(-7))
                .ToList(); // Önce belleğe çekelim, sonra işleyelim (Daha güvenli)

            // Her gün için kayıt sayısını hesapla
            var signupCounts = last7Days.Select(date =>
                userSignups.Count(u => u.registration_date.Date == date)
            ).ToArray();

            var dateLabels = last7Days.Select(d => d.ToString("dd MMM")).ToArray(); // "12 Dec" gibi format

            ViewBag.UserSignupLabels = dateLabels;
            ViewBag.UserSignupCounts = signupCounts;

            return View();
        }
    }
}