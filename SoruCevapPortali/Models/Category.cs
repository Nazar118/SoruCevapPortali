using System.ComponentModel.DataAnnotations;

namespace SoruCevapPortali.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kategori adı zorunludur.")]
        [StringLength(100)]
        public string Name { get; set; }

        // --- HATAYI ÇÖZEN KISIM BURASI ---
        // Controller'da "c.Questions" diyebilmemiz için bu listenin burada olması ve adının "Questions" olması şart.
        public virtual ICollection<Question>? Questions { get; set; }
    }
}