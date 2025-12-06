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
        public bool IsBestAnswer { get; set; }      // SQL: IsBestAnswer (Bu doğruydu)

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        public int QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public virtual Question? Question { get; set; }
    }
}