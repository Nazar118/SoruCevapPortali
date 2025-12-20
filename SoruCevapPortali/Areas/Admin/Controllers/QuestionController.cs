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
    [Authorize(Roles = "Admin")] // Güvenlik: Sadece Adminler girebilsin
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
            // FAZ 0 GÜNCELLEMESİ: Silinmemiş soruları getir
            var questions = _context.Questions
                                    .Include(q => q.User)
                                    .Where(q => q.IsDeleted == false)
                                    .OrderByDescending(q => q.creation_date)
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
                // FAZ 0 GÜNCELLEMESİ: Soft Delete (Gizleme)
                question.IsDeleted = true;
                _questionRepository.Update(question);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var question = _questionRepository.GetById(id);
            if (question == null) return NotFound();

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