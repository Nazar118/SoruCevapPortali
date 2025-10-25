using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // BU SATIRI EKLEDİĞİNDEN EMİN OL!
using SoruCevapPortali.Interfaces;
using SoruCevapPortali.Models;

namespace SoruCevapPortali.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class SoruController : Controller
    {
        private readonly IRepository<Soru> _soruRepository;
        // DbContext'i de buraya ekliyoruz çünkü Include işlemi için ona ihtiyacımız var.
        private readonly Data.ApplicationDbContext _context;

        public SoruController(IRepository<Soru> soruRepository, Data.ApplicationDbContext context)
        {
            _soruRepository = soruRepository;
            _context = context; // Eklendi
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
    }
}