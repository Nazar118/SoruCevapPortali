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

        // BU METOT ZATEN VARDI, KULLANICILARI LİSTELER
        public IActionResult Index()
        {
            var kullanicilar = _kullaniciRepository.GetAll();
            return View(kullanicilar);
        }
        // Boş "Yeni Kullanıcı Ekle" formunu göstermek için.
        // Adres: /Admin/Kullanici/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Dolu formdan gelen bilgileri kaydedip listeye yönlendirmek için.
        [HttpPost]
        [ValidateAntiForgeryToken] // Güvenlik için
        public IActionResult Create(Kullanici kullanici)
        {
            if (ModelState.IsValid)
            {
                kullanici.KayitTarihi = DateTime.Now; // Kayıt tarihini o an olarak ayarla
                _kullaniciRepository.Add(kullanici);
                return RedirectToAction(nameof(Index)); // Listeleme sayfasına geri dön
            }
            return View(kullanici); // Bir hata varsa formu tekrar göster
        }

        // "Sil" butonuna basıldığında çalışacak metot.
        [HttpPost]
        [ValidateAntiForgeryToken] // Güvenlik için
        public IActionResult Delete(int id)
        {
            var kullanici = _kullaniciRepository.GetById(id);
            if (kullanici != null)
            {
                _kullaniciRepository.Delete(kullanici);
            }
            return RedirectToAction(nameof(Index)); // Her durumda listeleme sayfasına geri dön
        }
    }
}