using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Include için eklendi
using SoruCevapPortali.Data; // DbContext için eklendi
using SoruCevapPortali.Models;
using System.Diagnostics;

namespace SoruCevapPortali.Controllers
{
    public class HomeController : Controller
    {
        // _logger yerine _context'i kullanýyoruz
        private readonly ApplicationDbContext _context;

        // Constructor'ý da _context'i alacak þekilde deðiþtiriyoruz
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Burasý sitenin ana sayfasý olacak (örn: https://localhost:7163/)
        public IActionResult Index(int? categoryId)
        {
            // 1. Sorularý Hazýrlama (Sorgu Baþlangýcý)
            var questionsQuery = _context.Questions
                .Include(q => q.User)
                .Include(q => q.Category)
                .Include(q => q.Answers)
                .AsQueryable();

            // 2. Eðer bir kategoriye týklanmýþsa FÝLTRELE
            if (categoryId.HasValue)
            {
                questionsQuery = questionsQuery.Where(q => q.CategoryId == categoryId);
            }

            // 3. Listeyi Çek (Tarihe göre en yeni en üstte)
            var questions = questionsQuery
                .OrderByDescending(q => q.creation_date)
                .ToList();

            // 4. Sidebar için kategorileri gönder
            ViewBag.Categories = _context.Categories.Include(c => c.Questions).ToList();

            // 5. Hangi kategori seçili? (View tarafýnda boyamak için lazým)
            ViewBag.SelectedCategoryId = categoryId;

            return View(questions);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}