using System.Linq.Expressions;

namespace SoruCevapPortali.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T GetById(int id);

        //  Filtre vererek çekebilme özelliği eklendi 
        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null);

        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(int id); // ID ile silme kolaylığı
        int Count(); // Sayı alma (Dashboard için lazım)
    }
}