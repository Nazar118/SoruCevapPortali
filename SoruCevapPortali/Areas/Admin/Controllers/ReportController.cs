using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoruCevapPortali.Data; // DbContext için
using SoruCevapPortali.Interfaces;
using SoruCevapPortali.Models;

namespace SoruCevapPortali.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ReportController : Controller
    {
        private readonly IRepository<Report> _reportRepository;
        private readonly IRepository<Question> _questionRepository;
        private readonly IRepository<Answer> _answerRepository;
        private readonly ApplicationDbContext _context; // <-- EKLENDİ

        // Constructor'ı Güncelledik: context parametresi eklendi
        public ReportController(IRepository<Report> reportRepository,
                                IRepository<Question> questionRepository,
                                IRepository<Answer> answerRepository,
                                ApplicationDbContext context) // <-- EKLENDİ
        {
            _reportRepository = reportRepository;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _context = context; // <-- EKLENDİ
        }

        public IActionResult Index()
        {
            var reports = _reportRepository.GetAll();
            return View(reports);
        }

        [HttpPost]
        public IActionResult MarkAsResolved(int id)
        {
            var report = _reportRepository.GetById(id);
            if (report != null)
            {
                report.is_resolved = !report.is_resolved;
                _reportRepository.Update(report);
            }
            return RedirectToAction(nameof(Index));
        }

        // Şikayet edilen içeriği (Soru veya Cevap) SİLME işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteContent(int reportId)
        {
            var report = _reportRepository.GetById(reportId);
            if (report == null) return NotFound();

            // 1. Önce Raporu Siliyoruz (Veritabanı kilidini açmak için)
            _reportRepository.Delete(report);

            // 2. Şimdi asıl içeriği siliyoruz
            if (report.QuestionId != null)
            {
                // Soruyu bul
                var question = _questionRepository.GetById(report.QuestionId.Value);

                // Eğer soruya bağlı başka raporlar varsa onları da temizle
                var relatedReports = _context.Reports.Where(r => r.QuestionId == report.QuestionId).ToList();
                if (relatedReports.Any())
                {
                    _context.Reports.RemoveRange(relatedReports);
                    _context.SaveChanges();
                }

                // Soruyu sil
                if (question != null) _questionRepository.Delete(question);
            }
            else if (report.AnswerId != null)
            {
                // Cevabı bul
                var answer = _answerRepository.GetById(report.AnswerId.Value);

                // Eğer cevaba bağlı başka raporlar varsa temizle
                var relatedReports = _context.Reports.Where(r => r.AnswerId == report.AnswerId).ToList();
                if (relatedReports.Any())
                {
                    _context.Reports.RemoveRange(relatedReports);
                    _context.SaveChanges();
                }

                // Cevabı sil
                if (answer != null) _answerRepository.Delete(answer);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}