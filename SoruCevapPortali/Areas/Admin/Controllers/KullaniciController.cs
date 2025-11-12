using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoruCevapPortali.Interfaces;
using SoruCevapPortali.Models;

namespace SoruCevapPortali.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class KullaniciController : Controller
    {
        private readonly IRepository<Kullanici> _kullaniciRepository;

        public KullaniciController(IRepository<Kullanici> kullaniciRepository)
        {
            _kullaniciRepository = kullaniciRepository;
        }

        // --- 1. Metot: Listeleme ---
        public IActionResult Index()
        {
            var kullanicilar = _kullaniciRepository.GetAll();
            return View(kullanicilar);
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
        public IActionResult Create(Kullanici kullanici)
        {
            if (ModelState.IsValid)
            {
                kullanici.KayitTarihi = DateTime.Now;
                _kullaniciRepository.Add(kullanici);
                return RedirectToAction(nameof(Index));
            }
            return View(kullanici);
        }

        // --- 4. Metot: Silme İşlemi (POST) ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var kullanici = _kullaniciRepository.GetById(id);
            if (kullanici != null)
            {
                _kullaniciRepository.Delete(kullanici);
            }
            return RedirectToAction(nameof(Index));
        }

        // --- 5. Metot: Düzenle Sayfasını Aç (GET) ---
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var kullanici = _kullaniciRepository.GetById(id);
            if (kullanici == null)
            {
                return NotFound();
            }
            return View(kullanici);
        }

        // --- 6. Metot: Düzenlemeyi Kaydet (POST) ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Kullanici kullanici)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _kullaniciRepository.GetById(kullanici.Id);
                if (existingUser == null)
                {
                    return NotFound();
                }

                existingUser.KullaniciAdi = kullanici.KullaniciAdi;
                existingUser.Email = kullanici.Email;
                existingUser.Sifre = kullanici.Sifre;
                _kullaniciRepository.Update(existingUser);

                return RedirectToAction(nameof(Index));
            }
            return View(kullanici);
        }

        // --- 7. Metot: AJAX ile Durum Değiştirme (POST) ---
        [HttpPost]
        public IActionResult ToggleStatus(int id)
        {
            var kullanici = _kullaniciRepository.GetById(id);
            if (kullanici == null)
            {
                return NotFound();
            }

            kullanici.AktifMi = !kullanici.AktifMi;
            _kullaniciRepository.Update(kullanici);

            return Json(new { success = true, isActive = kullanici.AktifMi });
        }

    } 
} 