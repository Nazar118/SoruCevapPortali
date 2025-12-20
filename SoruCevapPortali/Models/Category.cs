using System.ComponentModel.DataAnnotations;

namespace SoruCevapPortali.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kategori adı zorunludur.")]
        [StringLength(100)]
        public string Name { get; set; }

        // === YENİ EKLENEN: SİLİNDİ Mİ? (Pasif Kategori) ===
        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<Question>? Questions { get; set; }
    }
}