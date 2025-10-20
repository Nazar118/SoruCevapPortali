using SoruCevapPortali.Data;
using SoruCevapPortali.Interfaces;
using SoruCevapPortali.Models;

namespace SoruCevapPortali.Repositories
{
    public class KullaniciRepository : IRepository<Kullanici>
    {
        private readonly ApplicationDbContext _context;

        // Bu sınıfa çalışması için veritabanı bağlantısı (DbContext) veriliyor.
        public KullaniciRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Kullanici entity)
        {
            _context.Kullanicilar.Add(entity);
            _context.SaveChanges(); // Değişiklikleri veritabanına kaydet
        }

        public void Delete(Kullanici entity)
        {
            _context.Kullanicilar.Remove(entity);
            _context.SaveChanges();
        }

        public IEnumerable<Kullanici> GetAll()
        {
            return _context.Kullanicilar.ToList();
        }

        public Kullanici GetById(int id)
        {
            return _context.Kullanicilar.Find(id);
        }

        public void Update(Kullanici entity)
        {
            _context.Kullanicilar.Update(entity);
            _context.SaveChanges();
        }
    }
}