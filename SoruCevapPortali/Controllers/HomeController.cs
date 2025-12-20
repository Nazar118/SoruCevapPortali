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
            var query = _context.Questions
                .Where(q => q.IsDeleted == false && q.Is_it_approved == true) // Silinmemiþ ve Onaylýlar
                .AsQueryable();

            if (categoryId.HasValue)
            {
                query = query.Where(q => q.CategoryId == categoryId);
            }

            // ViewModel'e Çevirme Ýþlemi (Projection)
            var viewModels = query
                .OrderByDescending(q => q.creation_date)
                .Select(q => new QuestionListViewModel
                {
                    Id = q.Id,
                    Title = q.title,
                    // Ýçeriðin ilk 100 karakterini al, yoksa tamamýný al
                    ContentSummary = q.contents.Length > 100 ? q.contents.Substring(0, 100) + "..." : q.contents,
                    CategoryName = q.Category.Name,
                    CategoryId = q.Category.Id,
                    UserName = q.User.User_name,
                    AnswerCount = q.Answers.Count(), // Silinmemiþ cevaplarý saymak daha doðru olur ileride
                    CreatedDate = q.creation_date,
                    // Eðer cevaplardan herhangi biri "En Ýyi Cevap" ise soruyu Çözüldü say
                    IsSolved = q.Answers.Any(a => a.IsBestAnswer)
                })
                .ToList();

            // Sidebar kategorileri için
            ViewBag.Categories = _context.Categories
                .Include(c => c.Questions) 
                .Where(c => !c.IsDeleted)
                .ToList();

            // 1. Popüler Sorular (En çok cevabý olan ilk 5 soru)
            ViewBag.PopularQuestions = _context.Questions
                .Where(q => !q.IsDeleted && q.Is_it_approved)
                .OrderByDescending(q => q.Answers.Count())
                .Take(5)
                .Select(q => new QuestionListViewModel
                {
                    Id = q.Id,
                    Title = q.title,
                    AnswerCount = q.Answers.Count()
                })
                .ToList();

            // 2. Site Ýstatistikleri
            ViewBag.TotalQuestions = _context.Questions.Count(q => !q.IsDeleted && q.Is_it_approved);
            ViewBag.TotalAnswers = _context.Answers.Count(a => !a.IsDeleted);
            ViewBag.TotalUsers = _context.Users.Count();
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