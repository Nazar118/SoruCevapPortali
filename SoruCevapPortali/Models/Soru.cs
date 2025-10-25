// Burası Models/Soru.cs dosyasının içeriği
using System.ComponentModel.DataAnnotations.Schema;

namespace SoruCevapPortali.Models
{
    public class Soru
    {
        public int Id { get; set; }
        public string Baslik { get; set; }
        public string Icerik { get; set; }
        public DateTime OlusturmaTarihi { get; set; }

        // İlişkiyi kuruyoruz: Her sorunun bir kullanıcısı vardır.
        public int KullaniciId { get; set; }
        [ForeignKey("KullaniciId")]
        public virtual Kullanici SoranKullanici { get; set; }
        public virtual ICollection<Cevap> Cevaplar { get; set; }
    }
}