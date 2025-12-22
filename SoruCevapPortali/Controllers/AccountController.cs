using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SoruCevapPortali.Models;

namespace SoruCevapPortali.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // 1. GİRİŞ YAP SAYFASI
        [HttpGet]
        public IActionResult Login()
        {
            // Zaten giriş yapmışsa yönlendir
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Lütfen tüm alanları doldurun.";
                return View();
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = await _userManager.FindByNameAsync(email);
            }

            if (user != null)
            {
                if (user.PasswordHash == password)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);


                    if (user.IsAdmin) return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                    return RedirectToAction("Index", "Home");
                }

                var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

                if (result.Succeeded)
                {
                    if (user.IsAdmin) return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.Error = "Geçersiz kullanıcı adı/e-posta veya şifre.";
            return View();
        }


        // 3. KAYIT OL SAYFASI
        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }

        // 4. KAYIT OL İŞLEMİ (Identity ile Hashleyerek Kayıt)
        [HttpPost]
        public async Task<IActionResult> Register(User model)
        {
            // Identity User_name yerine UserName kullanır, onu eşleyelim
            model.UserName = model.User_name;

            // Validasyon kontrolü (Basitçe)
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                ViewBag.Error = "Lütfen bilgileri eksiksiz girin.";
                return View(model);
            }

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ViewBag.Error = "Bu e-posta zaten kullanılıyor.";
                return View(model);
            }

            // Yeni kullanıcı ayarları
            model.registration_date = DateTime.Now;
            model.Is_it_active = true;
            model.IsAdmin = false;
            // SecurityStamp Identity için gereklidir
            model.SecurityStamp = Guid.NewGuid().ToString();

            // Identity ile oluştur (Şifreyi otomatik Hashler!)
            var result = await _userManager.CreateAsync(model, model.Password);

            if (result.Succeeded)
            {
                // Başarılıysa giriş sayfasına yönlendir
                return RedirectToAction("Login");
            }
            else
            {
                // Identity'den dönen hataları göster (Örn: Şifre çok basit vs.)
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                ViewBag.Error = "Kayıt oluşturulamadı. Şifreniz en az 1 büyük harf, 1 küçük harf ve rakam içermelidir.";
            }

            return View(model);
        }

        // 5. ÇIKIŞ YAP (Identity Yöntemi)
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}