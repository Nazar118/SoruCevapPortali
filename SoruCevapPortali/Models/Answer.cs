using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoruCevapPortali.Models
{
    public class Answer
    {
        public int Id { get; set; }

        [Required]
        public string contents { get; set; } // SQL: contents

        public DateTime creation_date { get; set; } // SQL: creation_date
        public bool IsBestAnswer { get; set; }      // SQL: IsBestAnswer


        public bool IsApproved { get; set; } = true;

        // === YENİ EKLENEN: SİLİNDİ Mİ? ===
        public bool IsDeleted { get; set; } = false;

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        public int QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public virtual Question? Question { get; set; }

        public virtual ICollection<Report>? Reports { get; set; }
        [NotMapped]
        public bool IsLikedByCurrentUser { get; set; }

        [NotMapped]
        public int LikeCount { get; set; }
    }
}