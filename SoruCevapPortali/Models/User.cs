using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 

namespace SoruCevapPortali.Models
{
    public class User : IdentityUser<int>
    {
        public string? User_name { get; set; }
        public string? Password { get; set; }
        public DateTime registration_date { get; set; }
        public bool Is_it_active { get; set; }
        public bool IsAdmin { get; set; } = false;
        public int WarningCount { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<Question>? Questions { get; set; }

        // "Answer tablosundaki 'User' alanı ile bu listeyi eşleştir" diyoruz.
        [InverseProperty("User")]
        public virtual ICollection<Answer>? Answers { get; set; }
    }
}