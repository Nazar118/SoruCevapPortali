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

            switch (sortOrder)
            {
                case "popular":
                    query = query
                        .Where(q => q.Answers.Count(a => !a.IsDeleted) >= 3) 
                        .OrderByDescending(q => q.Answers.Count(a => !a.IsDeleted))
                        .ThenByDescending(q => q.creation_date);
                    break;

                case "oldest": 
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
                    // Ýçerik çok uzunsa kýsalt
                    ContentSummary = q.contents.Length > 100 ? q.contents.Substring(0, 100) + "..." : q.contents,
                    CategoryName = q.Category.Name,
                    CategoryId = q.Category.Id,
                    UserName = q.User.User_name,
                    AnswerCount = q.Answers.Count(a => !a.IsDeleted),
                    CreatedDate = q.creation_date,
                    IsSolved = q.Answers.Any(a => a.IsBestAnswer),

                    FeaturedAnswerContent = q.Answers
                        .Where(a => !a.IsDeleted && a.IsBestAnswer)
                        .Select(a => a.contents)
                        .FirstOrDefault()
                        ?? q.Answers
                        .Where(a => !a.IsDeleted)
                        .Select(a => a.contents)
                        .FirstOrDefault(),

                    // Ayný mantýkla Kullanýcý Adýný al
                    FeaturedAnswerUserName = q.Answers
                        .Where(a => !a.IsDeleted && a.IsBestAnswer)
                        .Select(a => a.User.User_name)
                        .FirstOrDefault()
                        ?? q.Answers
                        .Where(a => !a.IsDeleted)
                        .Select(a => a.User.User_name)
                        .FirstOrDefault(),

                    // Gösterilen cevap "En Ýyi Cevap" mý kontrol et
                    IsFeaturedAnswerBest = q.Answers.Any(a => !a.IsDeleted && a.IsBestAnswer)
                })
                .ToList();

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

        public IActionResult Akis()
        {
            var feedItems = new List<FeedItemViewModel>();

            // 1. SORU AKIÞI (Sadece Son 4 Soru - Kalabalýk yapmasýn)
            var questions = _context.Questions
                .Include(q => q.Category)
                .Include(q => q.User)
                .Where(q => !q.IsDeleted && q.Is_it_approved)
                .OrderByDescending(q => q.creation_date)
                .Take(4) // Sayýyý azalttýk
                .Select(q => new FeedItemViewModel
                {
                    Id = q.Id,
                    Type = FeedType.Question,
                    Title = q.title,
                    Content = q.contents,
                    UserName = q.User.User_name,
                    CategoryName = q.Category.Name,
                    Date = q.creation_date,
                    AnswerCount = q.Answers.Count(a => !a.IsDeleted)
                }).ToList();

            // 2. CEVAP AKIÞI (Sadece Son 3 Cevap)
            var answers = _context.Answers
                .Include(a => a.Question)
                .Include(a => a.User)
                .Where(a => !a.IsDeleted)
                .OrderByDescending(a => a.creation_date)
                .Take(3)
                .Select(a => new FeedItemViewModel
                {
                    Id = a.QuestionId,
                    Type = FeedType.Answer,
                    Title = "Yeni Bir Cevap",
                    Content = a.contents,
                    UserName = a.User.User_name,
                    CategoryName = a.Question.Category.Name,
                    Date = a.creation_date,
                    TargetQuestionTitle = a.Question.title
                }).ToList();

            var ilgincBilgilerHavuzu = new List<FeedItemViewModel>
            {
                new FeedItemViewModel { Type = FeedType.InfoCard, CategoryName = "Hayvanlar Alemi", Title = "Ahtapotlarýn Kalbi", Content = "Ahtapotlarýn üç tane kalbi vardýr. Biri vücuda kan pompalar, diðer ikisi solungaçlara.", UserName = "Bilgi Botu", Date = DateTime.Now },
                new FeedItemViewModel { Type = FeedType.InfoCard, CategoryName = "Bitkiler Alemi", Title = "Muz Bir Meyve Deðildir", Content = "Botanik açýdan muz bir meyve deðil, bir ottur. Çilek ise meyve deðil, çiçek tablasýdýr.", UserName = "Bilgi Botu", Date = DateTime.Now.AddMinutes(-5) },
                new FeedItemViewModel { Type = FeedType.InfoCard, CategoryName = "Saðlýk", Title = "Su Ýçmenin Önemi", Content = "Beynimizin %75'i sudan oluþur. Hafif bir susuzluk bile odaklanma sorunu ve baþ aðrýsý yapabilir.", UserName = "Sistem", Date = DateTime.Now.AddMinutes(-10) },
                new FeedItemViewModel { Type = FeedType.InfoCard, CategoryName = "Futbol", Title = "En Hýzlý Gol", Content = "Dünya futbol tarihindeki en hýzlý gol, baþlama düdüðünden sadece 2.8 saniye sonra atýlmýþtýr.", UserName = "Spor Servisi", Date = DateTime.Now.AddMinutes(-20) },
                new FeedItemViewModel { Type = FeedType.InfoCard, CategoryName = "Uzay", Title = "Uzayda Sessizlik", Content = "Uzayda atmosfer olmadýðý için ses dalgalarý yayýlamaz. Yani uzay tamamen sessizdir.", UserName = "Bilim Köþesi", Date = DateTime.Now.AddMinutes(-30) },
                new FeedItemViewModel { Type = FeedType.InfoCard, CategoryName = "Hayvanlar Alemi", Title = "Zürafalarýn Dili", Content = "Zürafalarýn dilleri o kadar uzundur ki (yaklaþýk 50cm), kulaklarýný bile kendi dilleriyle temizleyebilirler.", UserName = "Doða Rehberi", Date = DateTime.Now.AddMinutes(-40) },
                new FeedItemViewModel { Type = FeedType.InfoCard, CategoryName = "Eðitim", Title = "Beyin Kapasitesi", Content = "Ýnsan beyni, dünyadaki tüm telefonlarýn toplamýndan daha fazla iþlem yapma kapasitesine sahiptir.", UserName = "Akademi", Date = DateTime.Now.AddMinutes(-50) }
            };

            // Havuzdan Rastgele 3 Tane Seç
            var random = new Random();
            var secilenBilgiler = ilgincBilgilerHavuzu.OrderBy(x => random.Next()).Take(3).ToList();

            // HEPSÝNÝ BÝRLEÞTÝR VE KARIÞTIR
            feedItems.AddRange(questions);
            feedItems.AddRange(answers);
            feedItems.AddRange(secilenBilgiler);

            // Tarihe göre deðil de tamamen karýþýk (Shuffle) gelmesi akýþ hissini artýrýr
            var karisikAkis = feedItems.OrderByDescending(x => x.Date).ToList();

            return View(karisikAkis);
        }
        // --- RASTGELE SORUYU ANA SAYFA GÖRÜNÜMÜNDE GETÝR ---
        public IActionResult RandomQuestion()
        {
            // 1. Rastgele bir soru seç
            var q = _context.Questions
                .Include(x => x.Category)
                .Include(x => x.User)
                .Include(x => x.Answers)
                    .ThenInclude(a => a.User)
                .Where(x => !x.IsDeleted && x.Is_it_approved)
                .OrderBy(r => Guid.NewGuid()) // Rastgele sýrala
                .FirstOrDefault(); // Ýlkini al

            if (q == null) return RedirectToAction("Index");

            // 2. Bu soruyu, Ana Sayfanýn anladýðý 'ViewModel' formatýna çevir
            var tekSoruModeli = new QuestionListViewModel
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

                // Akýþ kýsmýndaki gibi cevap önizlemesi (Opsiyonel, þýk dursun diye ekledim)
                FeaturedAnswerContent = q.Answers.Where(a => !a.IsDeleted).Select(a => a.contents).FirstOrDefault(),
                FeaturedAnswerUserName = q.Answers.Where(a => !a.IsDeleted).Select(a => a.User.User_name).FirstOrDefault(),
                IsFeaturedAnswerBest = q.Answers.Any(a => !a.IsDeleted && a.IsBestAnswer)
            };

            // 3. Ana Sayfa (Index) bizden bir LÝSTE bekliyor. O yüzden tek soruyu listeye koyuyoruz.
            var modelListesi = new List<QuestionListViewModel> { tekSoruModeli };


            // --- SIDEBAR VERÝLERÝ (Burasý Mecbur, yoksa sol menü ve sað taraf boþ kalýr) ---
            ViewBag.Categories = _context.Categories.Include(c => c.Questions).Where(c => !c.IsDeleted).ToList();

            ViewBag.PopularQuestions = _context.Questions
                .Where(xq => !xq.IsDeleted && xq.Is_it_approved)
                .OrderByDescending(xq => xq.Answers.Count())
                .Take(5)
                .Select(xq => new QuestionListViewModel { Id = xq.Id, Title = xq.title, AnswerCount = xq.Answers.Count() })
                .ToList();

            ViewBag.TotalQuestions = _context.Questions.Count(xq => !xq.IsDeleted && xq.Is_it_approved);
            ViewBag.TotalAnswers = _context.Answers.Count(a => !a.IsDeleted);
            ViewBag.TotalUsers = _context.Users.Count();

            // Baþlýk bilgisini deðiþtirelim ki kullanýcý anlasýn
            ViewBag.CurrentSort = "Rastgele Soru";

            // 4. "Index" görünümünü (Ana Sayfa Tasarýmý) kullan ama bizim tek soruluk listeyi göster
            return View("Index", modelListesi);
        }
    }
}