// Burası Data/ApplicationDbContext.cs dosyasının son hali
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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Bir kullanıcı silinirse, ona ait soruların ve cevapların ne olacağı konusunda
            // SQL'in kafasının karışmasını önlemek için varsayılan "Cascade Delete" davranışını değiştiriyoruz.
            // Artık bir kullanıcının soruları veya cevapları varsa, o kullanıcı doğrudan silinemeyecek.

            modelBuilder.Entity<Soru>()
                .HasOne(s => s.SoranKullanici)
                .WithMany()
                .HasForeignKey(s => s.KullaniciId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cevap>()
                .HasOne(c => c.CevaplayanKullanici)
                .WithMany()
                .HasForeignKey(c => c.KullaniciId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}