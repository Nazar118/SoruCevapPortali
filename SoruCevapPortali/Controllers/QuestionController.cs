using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SoruCevapPortali.Data;
using SoruCevapPortali.Models;
using System.Security.Claims; // Kullanıcı ID'sini almak için

namespace SoruCevapPortali.Controllers
{
    public class QuestionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuestionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. SORU DETAY SAYFASI
        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();

            var question = _context.Questions
                .Include(q => q.User)      // Soruyu soran kullanıcı
                .Include(q => q.Category)  // Kategorisi
                .Include(q => q.Answers)   // Cevaplar
                    .ThenInclude(a => a.User) // Cevabı yazan kullanıcılar
                .FirstOrDefault(m => m.Id == id);

            if (question == null) return NotFound();

            return View(question);
        }

        // 2. SORU SORMA SAYFASI (Açılış)
        [HttpGet]
        public IActionResult Create()
        {
            // Eğer giriş yapmamışsa Login'e yönlendir
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Auth", new { area = "Admin" });
                // Not: AuthController senin Admin area içinde olduğu için area belirttik.
                // Eğer AuthController ana dizindeyse area kısmını silersin.
            }

            // Kategorileri Dropdown (Açılır Kutu) için hazırla
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // 3. SORU SORMA İŞLEMİ (Kaydetme)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("title,contents,CategoryId")] Question question)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Auth", new { area = "Admin" });
            }

            // Kullanıcı ID'sini bul ve soruya ekle
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");

            if (userId != null)
            {
                question.UserId = int.Parse(userId);
                question.creation_date = DateTime.Now;

                // Hoca onay mekanizması istediyse false yap, yoksa true kalsın.
                question.Is_it_approved = true;

                _context.Add(question);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            // Hata varsa kategori listesini tekrar doldur
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", question.CategoryId);
            return View(question);
        }
    }
}