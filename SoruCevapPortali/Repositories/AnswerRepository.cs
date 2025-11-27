using Microsoft.EntityFrameworkCore;
using SoruCevapPortali.Data;
using SoruCevapPortali.Interfaces;
using SoruCevapPortali.Models;

namespace SoruCevapPortali.Repositories
{
    public class AnswerRepository : IRepository<Answer>
    {
        private readonly ApplicationDbContext _context;
        public AnswerRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(Answer entity)
        {
            _context.Answers.Add(entity);
            _context.SaveChanges();
        }
        public void Delete(Answer entity)
        {
            _context.Answers.Remove(entity);
            _context.SaveChanges();
        }
        public IEnumerable<Answer> GetAll()
        {
            // .Include() ile Cevap'a bağlı olan CevaplayanKullanici ve AitOlduguSoru bilgilerini de çekiyoruz.
            return _context.Answers
                       .Include(c => c.User)
                       .Include(c => c.Question)
                       .ToList();
        }
        public Answer GetById(int id)
        {
            return _context.Answers.Find(id);
        }
        public void Update(Answer entity)
        { 
            _context.Answers.Update(entity);
            _context.SaveChanges();
        }
    }
}