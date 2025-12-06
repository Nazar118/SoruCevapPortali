using System.ComponentModel.DataAnnotations;

namespace SoruCevapPortali.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kategori adı zorunludur.")]
        [StringLength(100, ErrorMessage = "Kategori adı en fazla 100 karakter olabilir.")]
        public string Name { get; set; }

        // Bir kategorinin birden çok sorusu olabileceğini belirtiyoruz.
        public virtual ICollection<Question>? Question { get; set; }
    }
}