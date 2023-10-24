using System.ComponentModel.DataAnnotations;

namespace SPAmineseweeper.Models
{
    public class Score
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int HighScore { get; set; }
        public Player? Player { get; set; }
        public DateTime Date { get; set; }
    }
}
