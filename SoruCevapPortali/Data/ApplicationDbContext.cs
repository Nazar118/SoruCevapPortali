// Burası Data/ApplicationDbContext.cs dosyasının içeriği
using Microsoft.EntityFrameworkCore;
using SoruCevapPortali.Models;

namespace SoruCevapPortali.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Soru> Sorular { get; set; }
        public DbSet<Cevap> Cevaplar { get; set; }
    }
}