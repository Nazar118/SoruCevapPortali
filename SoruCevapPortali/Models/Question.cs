using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoruCevapPortali.Models
{
    public class Question
    {
        public int Id { get; set; }

        [Required]
        public string title { get; set; }

        [Required]
        public string contents { get; set; }

        public int? CategoryId { get; set; } // Eskiden KategoriId
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; } // Eskiden Kategori

        public DateTime creation_date { get; set; }
        public bool Is_ıt_approved { get; set; }

        // --- İŞTE BURALARI İNGİLİZCELEŞTİRİYORUZ ---
        public int UserId { get; set; } // Eskiden KullaniciId
        [ForeignKey("UserId")]
        public virtual User? User { get; set; } // Eskiden SoranKullanici

        public virtual ICollection<Answer>? Answers { get; set; } // Eskiden Cevaplar
    }
}