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


        // Rozetin üzerinde ne yazacak?
        public string StatusText
        {
            get
            {
                if (AnswerCount == 0) return "Cevap Bekliyor";
                if (AnswerCount > 5) return "Popüler";
                return "Cevaplandı";
            }
        }

        // Rozetin rengi ne olacak? (Bootstrap sınıfları)
        public string StatusClass
        {
            get
            {
                if (AnswerCount == 0) return "badge-warning text-dark"; // Sarı
                if (AnswerCount > 5) return "badge-danger";             // Kırmızı
                return "badge-success";                                 // Yeşil
            }
        }

        // İkon ne olacak?
        public string StatusIcon
        {
            get
            {
                if (AnswerCount == 0) return "fas fa-hourglass-half";
                if (AnswerCount > 5) return "fas fa-fire";
                return "fas fa-check";
            }
        }
    }
}