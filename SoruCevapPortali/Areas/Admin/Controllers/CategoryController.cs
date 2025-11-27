using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoruCevapPortali.Interfaces;
using SoruCevapPortali.Models;

namespace SoruCevapPortali.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly IRepository<Category> _kategoriRepository;

        public CategoryController(IRepository<Category> kategoriRepository)
        {
            _kategoriRepository = kategoriRepository;
        }

        // --- 1. Metot: Listeleme ---
        public IActionResult Index()
        {
            var kategoriler = _kategoriRepository.GetAll();
            return View(kategoriler);
        }

        // --- 2. Metot: Yeni Ekle Sayfasını Aç (GET) ---
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // --- 3. Metot: Yeni Eklemeyi Kaydet (POST) ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category kategori)
        {
            if (ModelState.IsValid)
            {
                _kategoriRepository.Add(kategori);
                return RedirectToAction(nameof(Index));
            }
            return View(kategori);
        }

        // --- 4. Metot: Düzenle Sayfasını Aç (GET) ---
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var kategori = _kategoriRepository.GetById(id);
            if (kategori == null)
            {
                return NotFound();
            }
            return View(kategori);
        }

        // --- 5. Metot: Düzenlemeyi Kaydet (POST) ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category kategori)
        {
            if (ModelState.IsValid)
            {
                _kategoriRepository.Update(kategori);
                return RedirectToAction(nameof(Index));
            }
            return View(kategori);
        }

        // --- 6. Metot: Silme İşlemi (POST) ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var kategori = _kategoriRepository.GetById(id);
            if (kategori != null)
            {
                _kategoriRepository.Delete(kategori);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}