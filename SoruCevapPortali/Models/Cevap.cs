// Models/Cevap.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoruCevapPortali.Models
{
    public class Cevap
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "İçerik zorunludur.")]
        public string Icerik { get; set; }

        public DateTime OlusturmaTarihi { get; set; }
        public bool EnIyiCevapMi { get; set; }

        public int KullaniciId { get; set; }
        [ForeignKey("KullaniciId")]
        public virtual Kullanici? CevaplayanKullanici { get; set; }

        public int SoruId { get; set; }
        [ForeignKey("SoruId")]
        public virtual Soru? AitOlduguSoru { get; set; }
    }
}