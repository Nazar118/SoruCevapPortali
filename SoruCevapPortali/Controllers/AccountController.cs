using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SoruCevapPortali.Data;
using SoruCevapPortali.Models;
using System.Security.Claims;

namespace SoruCevapPortali.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. GİRİŞ YAP SAYFASI
        [HttpGet]
        public IActionResult Login()
        {
            // Zaten giriş yapmışsa yönlendir
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin")) return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // 2. GİRİŞ YAP İŞLEMİ
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            // Kullanıcıyı bul
            var kullanici = _context.Users.FirstOrDefault(k => k.Email == email && k.Password == password);

            if (kullanici != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, kullanici.User_name),
                    new Claim(ClaimTypes.Email, kullanici.Email),
                    new Claim("UserId", kullanici.Id.ToString())
                };

                // Rol Atama
                if (kullanici.IsAdmin == true)
                {
                    claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, "User"));
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties();

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                // Yönlendirme Kontrolü
                if (kullanici.IsAdmin == true)
                {
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.Error = "Geçersiz e-posta veya şifre.";
            return View();
        }

        // 3. KAYIT OL SAYFASI
        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }

        // 4. KAYIT OL İŞLEMİ (Düzeltildi)
        [HttpPost]
        public async Task<IActionResult> Register(User model)
        {
            if (ModelState.IsValid)
            {
                var exists = _context.Users.Any(u => u.Email == model.Email || u.User_name == model.User_name);
                if (exists)
                {
                    ViewBag.Error = "Bu e-posta veya kullanıcı adı zaten kullanılıyor.";
                    return View(model);
                }

                model.registration_date = DateTime.Now;
                model.Is_it_active = true;
                model.IsAdmin = false;

                _context.Users.Add(model);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login");
            }
            return View(model);
        }

        // 5. ÇIKIŞ YAP
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}