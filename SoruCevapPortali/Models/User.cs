using System.ComponentModel.DataAnnotations;

namespace SoruCevapPortali.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string User_name { get; set; } // SQL: User_name

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public DateTime registration_date { get; set; } // SQL: registration_date
        public bool Is_it_active { get; set; }          // SQL: Is_it_active

        public virtual ICollection<Question>? Questions { get; set; }
        public virtual ICollection<Answer>? Answers { get; set; }

        public int WarningCount { get; set; } // Kullanıcının aldığı uyarı sayısı
    }
}