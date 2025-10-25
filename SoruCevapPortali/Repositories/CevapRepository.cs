using SoruCevapPortali.Data;
using SoruCevapPortali.Interfaces;
using SoruCevapPortali.Models;

namespace SoruCevapPortali.Repositories
{
    public class CevapRepository : IRepository<Cevap>
    {
        private readonly ApplicationDbContext _context;
        public CevapRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(Cevap entity)
        {
            _context.Cevaplar.Add(entity);
            _context.SaveChanges();
        }
        public void Delete(Cevap entity)
        {
            _context.Cevaplar.Remove(entity);
            _context.SaveChanges();
        }
        public IEnumerable<Cevap> GetAll()
        { 
        return _context.Cevaplar.ToList();
        }
        public Cevap GetById(int id)
        {
            return _context.Cevaplar.Find(id);
        }
        public void Update(Cevap entity)
        { 
            _context.Cevaplar.Update(entity);
            _context.SaveChanges();
        }
    }
}