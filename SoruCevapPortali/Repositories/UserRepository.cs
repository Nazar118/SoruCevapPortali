using SoruCevapPortali.Data;
using SoruCevapPortali.Interfaces;
using SoruCevapPortali.Models;

namespace SoruCevapPortali.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly ApplicationDbContext _context;

        // Bu sınıfa çalışması için veritabanı bağlantısı (DbContext) veriliyor.
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(User entity)
        {
            _context.Users.Add(entity);
            _context.SaveChanges(); // Değişiklikleri veritabanına kaydet
        }

        public void Delete(User entity)
        {
            _context.Users.Remove(entity);
            _context.SaveChanges();
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public void Update(User entity)
        {
            _context.Users.Update(entity);
            _context.SaveChanges();
        }
    }
}