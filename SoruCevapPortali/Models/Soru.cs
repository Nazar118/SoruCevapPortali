// Models/Soru.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoruCevapPortali.Models
{
    public class Soru
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur.")]
        public string Baslik { get; set; }

        [Required(ErrorMessage = "İçerik zorunludur.")]
        public string Icerik { get; set; }

        public int? KategoriId { get; set; }
        [ForeignKey("KategoriId")]
        public virtual Kategori? Kategori { get; set; }

        public DateTime OlusturmaTarihi { get; set; }

        public int KullaniciId { get; set; }
        [ForeignKey("KullaniciId")]
        public virtual Kullanici? SoranKullanici { get; set; }

        public virtual ICollection<Cevap>? Cevaplar { get; set; }
    }
}