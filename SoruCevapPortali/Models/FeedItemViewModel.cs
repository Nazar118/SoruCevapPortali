using System;

namespace SoruCevapPortali.Models
{
    // Akışta ne tür bir içerik olduğunu belirten etiketler
    public enum FeedType
    {
        Question,   // Normal Soru
        Answer,     // Biri bir şeye cevap verdi
        InfoCard    // (Eski Blog) En İyi Cevap seçilmiş, bilgi niteliğinde içerik
    }

    public class FeedItemViewModel
    {
        public int Id { get; set; } // Soru ID'si (Link vermek için)
        public FeedType Type { get; set; } // Kartın türü
        public string Title { get; set; } // Başlık
        public string Content { get; set; } // İçerik (Soru veya Cevap metni)
        public string UserName { get; set; } // Yazan kişi
        public string CategoryName { get; set; } // Kategori
        public DateTime Date { get; set; } // Sıralama için tarih

        // Ekstra Bilgiler
        public int AnswerCount { get; set; } // Soruysa kaç cevabı var?
        public string TargetQuestionTitle { get; set; } // Cevapsa, hangi soruya yazıldı?
    }
}