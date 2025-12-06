using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoruCevapPortali.Models
{
    public class Report
    {
        public int Id { get; set; }

        [Required]
        public string reason { get; set; } // Şikayet Sebebi

        public DateTime creation_date { get; set; } = DateTime.Now;
        public bool is_resolved { get; set; } // İncelendi mi?

        // Raporlayan (User_name, Email vb. buradan gelecek)
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User? Reporter { get; set; }

        // Şikayet edilen Soru
        public int? QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public virtual Question? Question { get; set; }

        // Şikayet edilen Cevap
        public int? AnswerId { get; set; }
        [ForeignKey("AnswerId")]
        public virtual Answer? Answer { get; set; }
    }
}