using SoruCevapPortali.Data;
using SoruCevapPortali.Interfaces;
using SoruCevapPortali.Models;

namespace SoruCevapPortali.Repositories
{
    public class KategoriRepository : IRepository<Kategori>
    {
        private readonly ApplicationDbContext _context;
        public KategoriRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Kategori entity)
        {
            _context.Kategoriler.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(Kategori entity)
        {
            _context.Kategoriler.Remove(entity);
            _context.SaveChanges();
        }

        public IEnumerable<Kategori> GetAll()
        {
            return _context.Kategoriler.ToList();
        }

        public Kategori GetById(int id)
        {
            return _context.Kategoriler.Find(id);
        }

        public void Update(Kategori entity)
        {
            _context.Kategoriler.Update(entity);
            _context.SaveChanges();
        }
    }
}