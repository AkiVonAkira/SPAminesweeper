using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPAmineseweeper.Models
{
    public class Score
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Game")]
        public int GameId { get; set; }
        [Required]
        public double HighScore { get; set; }
        public DateTime Date { get; set; }
        public virtual Game? Game { get; set; }
    }
}
