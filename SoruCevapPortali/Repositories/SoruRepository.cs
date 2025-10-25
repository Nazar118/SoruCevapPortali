using Microsoft.EntityFrameworkCore;
using SoruCevapPortali.Data;
using SoruCevapPortali.Interfaces;
using SoruCevapPortali.Models;

namespace SoruCevapPortali.Repositories
{
    public class SoruRepository : IRepository<Soru>
    {
        private readonly ApplicationDbContext _context;
        public SoruRepository(ApplicationDbContext context)
        {

            _context = context;
        }

        public void Add(Soru entity)
        {
            _context.Sorular.Add(entity);
            _context.SaveChanges();
        }
        public void Delete(Soru entity)
        {
            _context.Sorular.Remove(entity);
            _context.SaveChanges();
        }
        public IEnumerable<Soru> GetAll()
        {
            // .Include() ile Soru'ya bağlı olan SoranKullanici bilgisini de çekiyoruz.
            return _context.Sorular.Include(s => s.SoranKullanici).ToList();
        }
        public Soru GetById(int id)
        {
            return _context.Sorular.Find(id);
        }
        public void Update(Soru entity)
        {
            _context.Sorular.Update(entity);
            _context.SaveChanges();
        }
    }
}
