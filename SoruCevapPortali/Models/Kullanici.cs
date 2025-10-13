// Burası Models/Kullanici.cs dosyasının içeriği
namespace SoruCevapPortali.Models
{
    public class Kullanici
    {
        public int Id { get; set; } // Her kullanıcı için benzersiz bir numara
        public string KullaniciAdi { get; set; }
        public string Email { get; set; }
        public string Sifre { get; set; } // Şimdilik böyle, Identity ile daha güvenli yapacağız
        public DateTime KayitTarihi { get; set; }
    }
}