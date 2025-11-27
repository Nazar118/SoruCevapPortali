using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoruCevapPortali.Interfaces;
using SoruCevapPortali.Models;

namespace SoruCevapPortali.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IRepository<User> _userRepository;

        public UserController(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        // --- 1. Listeleme ---
        public IActionResult Index()
        {
            var users = _userRepository.GetAll();
            return View(users);
        }

        // --- 2. Yeni Ekle (GET) ---
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // --- 3. Yeni Ekle (POST) ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                user.registration_date = DateTime.Now;
                _userRepository.Add(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // --- 4. Silme ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var user = _userRepository.GetById(id);
            if (user != null)
            {
                _userRepository.Delete(user);
            }
            return RedirectToAction(nameof(Index));
        }

        // --- 5. Düzenle (GET) ---
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // --- 6. Düzenle (POST) ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _userRepository.GetById(user.Id);
                if (existingUser == null)
                {
                    return NotFound();
                }

                existingUser.User_name = user.User_name;
                existingUser.Email = user.Email;
                existingUser.Password = user.Password;

                _userRepository.Update(existingUser);

                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // --- 7. Durum Değiştir (AJAX) ---
        [HttpPost]
        public IActionResult ToggleStatus(int id)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Is_it_active = !user.Is_it_active;
            _userRepository.Update(user);

            return Json(new { success = true, isActive = user.Is_it_active });
        }
    }
}