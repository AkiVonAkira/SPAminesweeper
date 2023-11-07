using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPAmineseweeper.Models
{
    public class Score
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public double HighScore { get; set; }

        [ForeignKey("User")]
        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }
        public DateTime Date { get; set; }
    }
}
