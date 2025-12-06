using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SoruCevapPortali.Data;
using SoruCevapPortali.Interfaces;
using SoruCevapPortali.Models;

namespace SoruCevapPortali.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class QuestionController : Controller
    {
        private readonly IRepository<Question> _questionRepository;
        private readonly ApplicationDbContext _context;
        private readonly IRepository<Category> _categoryRepository;

        public QuestionController(IRepository<Question> questionRepository, ApplicationDbContext context, IRepository<Category> categoryRepository)
        {
            _questionRepository = questionRepository;
            _context = context;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            // DÜZELTME: SoranKullanici -> User
            var questions = _context.Questions
                                    .Include(q => q.User)
                                    .ToList();
            return View(questions);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var question = _questionRepository.GetById(id);
            if (question != null)
            {
                _questionRepository.Delete(question);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var question = _questionRepository.GetById(id);
            if (question == null) return NotFound();

            // DÜZELTME: KategoriId -> CategoryId (Eğer modelde değiştirdiysen)
            // Eğer modelde hala KategoriId ise burayı KategoriId yap. 
            // Ama biz CategoryId yapmıştık diye hatırlıyorum.
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "Id", "Name", question.CategoryId);

            return View(question);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Question question)
        {
            if (ModelState.IsValid)
            {
                _questionRepository.Update(question);
                return RedirectToAction(nameof(Index));
            }
            // DÜZELTME: KategoriId -> CategoryId
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "Id", "Name", question.CategoryId);
            return View(question);
        }

        [HttpPost]
        public IActionResult ToggleOnay(int id)
        {
            var question = _questionRepository.GetById(id);
            if (question == null) return NotFound();

            question.Is_it_approved = !question.Is_it_approved;
            _questionRepository.Update(question);

            return Json(new { success = true, isOnayli = question.Is_it_approved });
        }
    }
}