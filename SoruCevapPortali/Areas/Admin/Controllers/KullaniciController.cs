using Microsoft.AspNetCore.Mvc;
using SoruCevapPortali.Interfaces; // IRepository'yi kullanabilmek için ekledik
using SoruCevapPortali.Models;      // Kullanici modelini kullanabilmek için ekledik

namespace SoruCevapPortali.Areas.Admin.Controllers
{
    [Area("Admin")] // Bu controller'ın Admin Area'sına ait olduğunu belirtiyoruz.
    public class KullaniciController : Controller
    {
        // Veritabanı işlemleri için repository'mizi tutacak olan değişken
        private readonly IRepository<Kullanici> _kullaniciRepository;

        // Constructor Metot: Bu Controller oluşturulduğunda, .NET bize otomatik olarak
        // Program.cs'de kaydettiğimiz KullaniciRepository'yi "veriyor".
        // Buna "Dependency Injection" denir.
        public KullaniciController(IRepository<Kullanici> kullaniciRepository)
        {
            _kullaniciRepository = kullaniciRepository;
        }

        // Bu metot, /Admin/Kullanici veya /Admin/Kullanici/Index adresine gidildiğinde çalışacak.
        public IActionResult Index()
        {
            // Repository'yi kullanarak veritabanındaki TÜM kullanıcıları çekiyoruz.
            var kullanicilar = _kullaniciRepository.GetAll();

            // Çektiğimiz kullanıcı listesini View'a (görünüme) gönderiyoruz.
            return View(kullanicilar);
        }
    }
}