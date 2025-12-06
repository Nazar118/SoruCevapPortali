using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoruCevapPortali.Models
{
    public class Question
    {
        public int Id { get; set; }

        [Required]
        public string title { get; set; }    // SQL: title

        [Required]
        public string contents { get; set; } // SQL: contents

        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        public DateTime creation_date { get; set; } // SQL: creation_date
        public bool Is_it_approved { get; set; }    // SQL: Is_it_approved

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        public virtual ICollection<Answer>? Answers { get; set; }
    }
}