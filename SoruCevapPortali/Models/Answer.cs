using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoruCevapPortali.Models
{
    public class Answer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Content is required.")] // Mesajları da İngilizce yapabiliriz veya Türkçe kalabilir
        public string contents { get; set; } // Icerik -> Content yapabiliriz ama şimdilik Icerik kalsın, karışmasın.

        public DateTime creation_date { get; set; }
        public bool Is_it_the_best_answer { get; set; }

        // --- İŞTE BURALARI İNGİLİZCELEŞTİRİYORUZ ---
        public int UserId { get; set; } // Eskiden KullaniciId
        [ForeignKey("UserId")]
        public virtual User? User { get; set; } // Eskiden CevaplayanKullanici

        public int QuestionId { get; set; } // Eskiden SoruId
        [ForeignKey("QuestionId")]
        public virtual Question? Question { get; set; } // Eskiden AitOlduguSoru
    }
}