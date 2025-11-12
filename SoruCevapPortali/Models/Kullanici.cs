// Models/Kullanici.cs
using System.ComponentModel.DataAnnotations;

namespace SoruCevapPortali.Models
{
    public class Kullanici
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        public string KullaniciAdi { get; set; }

        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        public string Sifre { get; set; }

        public DateTime KayitTarihi { get; set; } // Hatanın çözümü bu satır!
        public bool AktifMi { get; set; }

        public virtual ICollection<Soru>? Sorular { get; set; }
        public virtual ICollection<Cevap>? Cevaplar { get; set; }
    }
}