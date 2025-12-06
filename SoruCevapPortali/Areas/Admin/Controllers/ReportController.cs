using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public ReportController(IRepository<Report> reportRepository, IRepository<Question> questionRepository, IRepository<Answer> answerRepository)
        {
            _reportRepository = reportRepository;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
        }

        public IActionResult Index()
        {
            // Tüm raporları getir (Repository'de Include'lar yapılmıştı)
            var reports = _reportRepository.GetAll();
            return View(reports);
        }

        // Raporu "İncelendi" olarak işaretle (Silmeden kapatmak için)
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

            // Eğer şikayet edilen bir SORU ise
            if (report.QuestionId != null)
            {
                var question = _questionRepository.GetById(report.QuestionId.Value);
                if (question != null) _questionRepository.Delete(question);
            }
            // Eğer şikayet edilen bir CEVAP ise
            else if (report.AnswerId != null)
            {
                var answer = _answerRepository.GetById(report.AnswerId.Value);
                if (answer != null) _answerRepository.Delete(answer);
            }

            // İçerik silindikten sonra raporu da "Çözüldü" yapalım veya silelim
            // Biz şimdilik raporu silelim ki liste temizlensin
            _reportRepository.Delete(report);

            return RedirectToAction(nameof(Index));
        }
    }
}