using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPAmineseweeper.Models
{
    public class Game
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime? GameStarted { get; set; }
        public DateTime? GameEnded { get; set; }
        public double Score { get; set; }
        [Required]
        public int BoardSize { get; set; }
        [Required]
        public int BombPercentage { get; set; }
        [Required]
        public string? Difficulty { get; set; }
        public List<Tile>? Tiles { get; set; }

        [ForeignKey("User")]
        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }
    }
}
