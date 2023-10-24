using System.ComponentModel.DataAnnotations;

namespace SPAmineseweeper.Models
{
    public class Board
    {
        [Key]
        public int Id { get; set; }
        public int BoardSize { get; set; }
        public string? Difficulty { get; set; }
        [Required]
        public int BombPercentage { get; set; }
        public List<Tile>? Tiles { get; set; }
    }
}
