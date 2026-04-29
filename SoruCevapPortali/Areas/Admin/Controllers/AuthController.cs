using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SoruCevapPortali.Areas.Admin.ViewModels;
using SoruCevapPortali.Data;
using SoruCevapPortali.Models;
using System.Security.Claims;

namespace SoruCevapPortali.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var kullanici = _context.Users.FirstOrDefault(k => k.Email == model.Email && k.Password == model.Sifre);

                if (kullanici != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, kullanici.User_name),
                        new Claim(ClaimTypes.Email, kullanici.Email),
                        new Claim("UserId", kullanici.Id.ToString())
                    };

                    if (kullanici.IsAdmin == true)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                    }
                    else
                    {
                        claims.Add(new Claim(ClaimTypes.Role, "User"));
                    }

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties { };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                    TempData["ShowWelcomeAnimation"] = true;

                    if (kullanici.IsAdmin == true)
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home", new { area = "" });
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Geçersiz e-posta veya şifre.");
                    TempData["ErrorMessage"] = "E-posta veya şifre hatalı!";
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User user)
        {
            var existingUser = _context.Users
                .FirstOrDefault(u => u.Email == user.Email || u.User_name == user.User_name);

            if (existingUser != null)
            {
                ModelState.AddModelError("", "Bu e-posta veya kullanıcı adı zaten kullanılıyor.");
                return View(user);
            }

            user.registration_date = DateTime.Now;
            user.Is_it_active = true;

            user.IsAdmin = false; // Yeni kayıt olanlar yönetici olamaz!
            

            _context.Users.Add(user);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Kayıt başarılı! Şimdi giriş yapabilirsiniz.";
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}