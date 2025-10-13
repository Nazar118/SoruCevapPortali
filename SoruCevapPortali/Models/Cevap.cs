// Burası Models/Cevap.cs dosyasının içeriği
using System.ComponentModel.DataAnnotations.Schema;

namespace SoruCevapPortali.Models
{
    public class Cevap
    {
        public int Id { get; set; }
        public string Icerik { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
        public bool EnIyiCevapMi { get; set; } // Soru sahibi bir cevabı en iyi olarak işaretleyebilir

        // İlişkileri kuruyoruz: Her cevabın bir kullanıcısı ve bir sorusu vardır.
        public int KullaniciId { get; set; }
        [ForeignKey("KullaniciId")]
        public virtual Kullanici CevaplayanKullanici { get; set; }

        public int SoruId { get; set; }
        [ForeignKey("SoruId")]
        public virtual Soru AitOlduguSoru { get; set; }
    }
}