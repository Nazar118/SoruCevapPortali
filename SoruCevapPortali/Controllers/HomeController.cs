using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoruCevapPortali.Data;
using SoruCevapPortali.Models;
using System.Diagnostics;

namespace SoruCevapPortali.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GÜNCELLENDÝ: Artýk sýralama (sortOrder) ve durum (status) parametreleri de alýyor
        public IActionResult Index(string search, int? categoryId, string sortOrder, string status)
        {
            // Filtre seçimlerini View'da hatýrlamak için ViewBag'e atýyoruz
            ViewBag.CurrentSearch = search;
            ViewBag.SelectedCategoryId = categoryId;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CurrentStatus = status;

            // 1. Temel Sorgu: Silinmemiþ ve Onaylanmýþ sorular
            var query = _context.Questions
                .Include(q => q.Category)
                .Include(q => q.User)
                .Include(q => q.Answers)
                .Where(q => !q.IsDeleted && q.Is_it_approved)
                .AsQueryable();

            // 2. Kategori Filtresi
            if (categoryId.HasValue)
            {
                query = query.Where(q => q.CategoryId == categoryId);
            }

            // 3. Arama Filtresi
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(q => q.title.Contains(search) || q.contents.Contains(search));
            }

            // --- YENÝ EKLENEN KISIM: DURUM FÝLTRESÝ (Cevapsýz / Çözüldü) ---
            if (!string.IsNullOrEmpty(status))
            {
                switch (status)
                {
                    case "unanswered": // Hiç cevabý olmayanlar
                        query = query.Where(q => q.Answers.Count == 0);
                        break;
                    case "solved": // En az bir tane "En Ýyi Cevap" seçilmiþ olanlar
                        query = query.Where(q => q.Answers.Any(a => a.IsBestAnswer));
                        break;
                }
            }

            // --- YENÝ EKLENEN KISIM: SIRALAMA (Popüler / En Yeni) ---
            switch (sortOrder)
            {
                case "popular": // En çok cevaplananlar en üstte
                    query = query.OrderByDescending(q => q.Answers.Count).ThenByDescending(q => q.creation_date);
                    break;
                case "oldest": // En eskiler
                    query = query.OrderBy(q => q.creation_date);
                    break;
                default: // Varsayýlan: En Yeni (Newest)
                    query = query.OrderByDescending(q => q.creation_date);
                    break;
            }

            // 4. ViewModel'e Çevir
            var viewModels = query
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

            // --- SIDEBAR VERÝLERÝ (Aynen Korundu) ---

            // Kategoriler
            ViewBag.Categories = _context.Categories
                .Include(c => c.Questions)
                .Where(c => !c.IsDeleted)
                .ToList();

            // Popüler Sorular (Sidebar için)
            ViewBag.PopularQuestions = _context.Questions
                .Where(q => !q.IsDeleted && q.Is_it_approved)
                .OrderByDescending(q => q.Answers.Count())
                .Take(5)
                .Select(q => new QuestionListViewModel { Id = q.Id, Title = q.title, AnswerCount = q.Answers.Count() })
                .ToList();

            // Ýstatistikler
            ViewBag.TotalQuestions = _context.Questions.Count(q => !q.IsDeleted && q.Is_it_approved);
            ViewBag.TotalAnswers = _context.Answers.Count(a => !a.IsDeleted);
            ViewBag.TotalUsers = _context.Users.Count();

            return View(viewModels);
        }

        // --- YENÝ EKLENEN METOT: RASTGELE SORU ---
        public IActionResult RandomQuestion()
        {
            // Veritabanýndan rastgele bir soru seç
            var randomQuestion = _context.Questions
                .Where(q => !q.IsDeleted && q.Is_it_approved)
                // SQL Server'da Guid.NewGuid() verileri rastgele sýralar
                .OrderBy(r => Guid.NewGuid())
                .FirstOrDefault();

            if (randomQuestion != null)
            {
                // Soru varsa detay sayfasýna yönlendir
                return RedirectToAction("Details", "Question", new { id = randomQuestion.Id });
            }

            // Soru yoksa ana sayfaya dön
            return RedirectToAction("Index");
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