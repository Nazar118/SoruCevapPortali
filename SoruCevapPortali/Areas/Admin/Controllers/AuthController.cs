// Burası Areas/Admin/Controllers/AuthController.cs dosyasının içeriği
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SoruCevapPortali.Areas.Admin.ViewModels;
using SoruCevapPortali.Data; // DbContext için
using System.Security.Claims;

namespace SoruCevapPortali.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context; // Veritabanı işlemleri için

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Login sayfasını göstermek için
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Login formundan gelen bilgileri işlemek için
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var kullanici = _context.Kullanicilar.FirstOrDefault(k => k.Email == model.Email && k.Sifre == model.Sifre);

                if (kullanici != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, kullanici.KullaniciAdi),
                        new Claim(ClaimTypes.Email, kullanici.Email),
                        new Claim("UserId", kullanici.Id.ToString())
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties { };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    return RedirectToAction("Index", "Kullanici");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Geçersiz e-posta veya şifre.");
                }
            }
            return View(model);
        }

        // Çıkış yapmak için
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}