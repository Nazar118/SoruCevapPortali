using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoruCevapPortali.Models
{
    public class AnswerLike
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public int AnswerId { get; set; }
        [ForeignKey("AnswerId")]
        public virtual Answer Answer { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}