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
        // Index Artýk hem kategori hem de arama kelimesi alýyor
        public IActionResult Index(string search, int? categoryId)
        {
            // 1. Temel Sorgu: Silinmemiþ ve Onaylanmýþ sorular
            var query = _context.Questions
                .Include(q => q.Category)
                .Include(q => q.User)
                .Include(q => q.Answers) // Cevap sayýlarý için lazým
                .Where(q => !q.IsDeleted && q.Is_it_approved)
                .AsQueryable();

            // 2. Kategori Seçilmiþse Filtrele
            if (categoryId.HasValue)
            {
                query = query.Where(q => q.CategoryId == categoryId);
            }

            // 3. ARAMA YAPILMIÞSA FÝLTRELE (YENÝ KISIM) ??
            if (!string.IsNullOrEmpty(search))
            {
                // Baþlýkta VEYA Ýçerikte aranan kelime geçiyor mu?
                query = query.Where(q => q.title.Contains(search) || q.contents.Contains(search));
            }

            // 4. ViewModel'e Çevir
            var viewModels = query
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
                    IsSolved = q.Answers.Any(a => a.IsBestAnswer)
                })
                .ToList();

            // 5. Sidebar Verileri
            ViewBag.Categories = _context.Categories
                .Include(c => c.Questions)
                .Where(c => !c.IsDeleted)
                .ToList();

            // Popüler Sorular (Sidebar)
            ViewBag.PopularQuestions = _context.Questions
                .Where(q => !q.IsDeleted && q.Is_it_approved)
                .OrderByDescending(q => q.Answers.Count())
                .Take(5)
                .Select(q => new QuestionListViewModel { Id = q.Id, Title = q.title, AnswerCount = q.Answers.Count() })
                .ToList();

            // Ýstatistikler (Sidebar)
            ViewBag.TotalQuestions = _context.Questions.Count(q => !q.IsDeleted && q.Is_it_approved);
            ViewBag.TotalAnswers = _context.Answers.Count(a => !a.IsDeleted);
            ViewBag.TotalUsers = _context.Users.Count();

            // View'a Bilgi Gönder (Arama kutusunda aranan kelime dursun diye)
            ViewBag.CurrentSearch = search;
            ViewBag.SelectedCategoryId = categoryId;

            return View(viewModels);
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