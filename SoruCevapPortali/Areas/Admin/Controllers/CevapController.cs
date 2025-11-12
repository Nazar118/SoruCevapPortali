using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoruCevapPortali.Interfaces;
using SoruCevapPortali.Models;

namespace SoruCevapPortali.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CevapController : Controller
    {
        private readonly IRepository<Cevap> _cevapRepository;

        public CevapController(IRepository<Cevap> cevapRepository)
        {
            _cevapRepository = cevapRepository;
        }

        // Bütün cevapları listeleyecek sayfa
        public IActionResult Index()
        {
            var cevaplar = _cevapRepository.GetAll();
            return View(cevaplar);
        }

        // Bir cevabı silmek için
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var cevap = _cevapRepository.GetById(id);
            if (cevap != null)
            {
                _cevapRepository.Delete(cevap);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}