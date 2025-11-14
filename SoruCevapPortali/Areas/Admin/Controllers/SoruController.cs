using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore; // BU SATIRI EKLEDİĞİNDEN EMİN OL!
using SoruCevapPortali.Interfaces;
using SoruCevapPortali.Models;
using SoruCevapPortali.Repositories;

namespace SoruCevapPortali.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class SoruController : Controller
    {
        private readonly IRepository<Soru> _soruRepository;
        // DbContext'i de buraya ekliyoruz çünkü Include işlemi için ona ihtiyacımız var.
        private readonly Data.ApplicationDbContext _context;
        private readonly IRepository<Kategori> _kategoriRepository;

        public SoruController(IRepository<Soru> soruRepository, Data.ApplicationDbContext context, IRepository<Kategori> kategoriRepository)
        {
            _soruRepository = soruRepository;
            _context = context;
            _kategoriRepository = kategoriRepository; // <-- ARTIK BU SATIR GEÇERLİ!
        }

        // Bütün soruları listeleyecek sayfa
        public IActionResult Index()
        {
            // Soruları çekerken, onlara bağlı olan Kullanıcı bilgilerini de getirelim.
            var sorular = _context.Sorular.Include(s => s.SoranKullanici).ToList();
            return View(sorular);
        }

        // Bir soruyu silmek için
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var soru = _soruRepository.GetById(id);
            if (soru != null)
            {
                _soruRepository.Delete(soru);
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var soru = _soruRepository.GetById(id);
            if (soru == null)
            {
                return NotFound();
            }

            // View'a göndereceğimiz kategori listesini hazırlıyoruz.
            // ViewBag, Controller'dan View'a küçük veriler taşımanın en kolay yoludur.
            ViewBag.Kategoriler = new SelectList(_kategoriRepository.GetAll(), "Id", "Ad", soru.KategoriId);

            return View(soru);
        }

        // DÜZENLEME FORMUNDAN GELEN BİLGİLERİ KAYDETMEK İÇİN (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Soru soru)
        {
            if (ModelState.IsValid)
            {
                _soruRepository.Update(soru);
                return RedirectToAction(nameof(Index));
            }

            // Eğer ModelState geçerli değilse (hata varsa),
            // sayfayı tekrar açmak için kategori listesini TEKRAR doldurmamız gerekir!
            ViewBag.Kategoriler = new SelectList(_kategoriRepository.GetAll(), "Id", "Ad", soru.KategoriId);
            return View(soru);
        }
    }
}