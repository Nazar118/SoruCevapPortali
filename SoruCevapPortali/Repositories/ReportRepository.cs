using Microsoft.EntityFrameworkCore;
using SoruCevapPortali.Data;
using SoruCevapPortali.Interfaces;
using SoruCevapPortali.Models;

namespace SoruCevapPortali.Repositories
{
    public class ReportRepository : IRepository<Report>
    {
        private readonly ApplicationDbContext _context;

        public ReportRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Report entity)
        {
            _context.Reports.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(Report entity)
        {
            _context.Reports.Remove(entity);
            _context.SaveChanges();
        }

        public IEnumerable<Report> GetAll()
        {
            // Raporları çekerken kim şikayet etmiş, neyi şikayet etmiş hepsini getiriyoruz
            return _context.Reports
                           .Include(r => r.Reporter)
                           .Include(r => r.Question)
                           .Include(r => r.Answer)
                           .OrderByDescending(r => r.creation_date) // En yeniler üstte
                           .ToList();
        }

        public Report GetById(int id)
        {
            return _context.Reports
                           .Include(r => r.Reporter)
                           .Include(r => r.Question)
                           .Include(r => r.Answer)
                           .FirstOrDefault(r => r.Id == id);
        }

        public void Update(Report entity)
        {
            _context.Reports.Update(entity);
            _context.SaveChanges();
        }
    }
}