// Models/Kullanici.cs
using System.ComponentModel.DataAnnotations;

namespace SoruCevapPortali.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        public string User_name { get; set; }

        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        public string Password { get; set; }

        public DateTime registration_date { get; set; } // Hatanın çözümü bu satır!
        public bool Is_it_active { get; set; }

        public virtual ICollection<Question>? Sorular { get; set; }
        public virtual ICollection<Answer>? Cevaplar { get; set; }
    }
}