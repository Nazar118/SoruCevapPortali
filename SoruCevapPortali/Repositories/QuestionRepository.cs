using Microsoft.EntityFrameworkCore;
using SoruCevapPortali.Data;
using SoruCevapPortali.Interfaces;
using SoruCevapPortali.Models;

namespace SoruCevapPortali.Repositories
{
    public class QuestionRepository : IRepository<Question>
    {
        private readonly ApplicationDbContext _context;
        public QuestionRepository(ApplicationDbContext context)
        {

            _context = context;
        }

        public void Add(Question entity)
        {
            _context.Questions.Add(entity);
            _context.SaveChanges();
        }
        public void Delete(Question entity)
        {
            _context.Questions.Remove(entity);
            _context.SaveChanges();
        }
        public IEnumerable<Question> GetAll()
        {
            // .Include() ile Soru'ya bağlı olan SoranKullanici bilgisini de çekiyoruz.
            return _context.Questions.Include(s => s.User).ToList();
        }
        public Question GetById(int id)
        {
            return _context.Questions.Find(id);
        }
        public void Update(Question entity)
        {
            _context.Questions.Update(entity);
            _context.SaveChanges();
        }
    }
}
