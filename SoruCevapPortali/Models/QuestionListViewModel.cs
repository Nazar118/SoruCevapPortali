using System;

namespace SoruCevapPortali.Models
{
    public class QuestionListViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ContentSummary { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public string UserName { get; set; }
        public string UserImage { get; set; }
        public int AnswerCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsSolved { get; set; }
        public string FeaturedAnswerContent { get; set; } // Gösterilecek cevabın içeriği
        public string FeaturedAnswerUserName { get; set; } // Cevabı yazan kişi
        public bool IsFeaturedAnswerBest { get; set; } // Bu cevap "En İyi" mi?


        // 1. StatusText için özel alan (Backing Field)
        private string _statusText;
        public string StatusText
        {
            get
            {
                // Eğer dışarıdan (Controller'dan) özel bir yazı atandıysa onu göster
                if (!string.IsNullOrEmpty(_statusText))
                    return _statusText;

                // Atanmadıysa standart mantığı çalıştır
                if (AnswerCount == 0) return "Cevap Bekliyor";
                if (AnswerCount > 5) return "Popüler";
                return "Cevaplandı";
            }
            set
            {
                // Dışarıdan değer atanabilmesi için set bloğu eklendi
                _statusText = value;
            }
        }

        // 2. StatusClass için özel alan
        private string _statusClass;
        public string StatusClass
        {
            get
            {
                if (!string.IsNullOrEmpty(_statusClass))
                    return _statusClass;

                if (AnswerCount == 0) return "badge-warning text-dark";
                if (AnswerCount > 5) return "badge-danger";
                return "badge-success";
            }
            set { _statusClass = value; }
        }

        // 3. StatusIcon için özel alan
        private string _statusIcon;
        public string StatusIcon
        {
            get
            {
                if (!string.IsNullOrEmpty(_statusIcon))
                    return _statusIcon;

                if (AnswerCount == 0) return "fas fa-hourglass-half";
                if (AnswerCount > 5) return "fas fa-fire";
                return "fas fa-check";
            }
            set { _statusIcon = value; }
        }
        // --- DÜZELTİLEN KISIM BİTİŞİ ---
    }
}