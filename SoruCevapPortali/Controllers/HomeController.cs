using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Include için eklendi
using SoruCevapPortali.Data; // DbContext için eklendi
using SoruCevapPortali.Models;
using System.Diagnostics;

namespace SoruCevapPortali.Controllers
{
    public class HomeController : Controller
    {
        // _logger yerine _context'i kullanýyoruz
        private readonly ApplicationDbContext _context;

        // Constructor'ý da _context'i alacak þekilde deðiþtiriyoruz
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Burasý sitenin ana sayfasý olacak (örn: https://localhost:7163/)
        public IActionResult Index()
        {
            var Questions
                = _context.Questions
                                  .Include(s => s.User)
                                  .Include(s => s.Answers)
                                  .Include(s => s.Category)
                                  .Where(s => s.Is_it_approved == true) // Sadece onaylý sorular
                                  .OrderByDescending(s => s.creation_date)
                                  .ToList();

            return View(Questions); // Modeli View'a gönder
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