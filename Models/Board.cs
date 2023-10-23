using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPAmineseweeper.Models
{
    public class Board
    {
        [Key]
        public int Id { get; set; }
       // [ForeignKey("Game")] is a data annotation that tells the database that the GameId property is a foreign key to the Game table.
        public int GameId { get; set; }
        [Required]
        public int Height { get; set; }
        [Required]
        public int Width { get; set; }
        [Required]
        public string? Difficulty { get; set; }
        [Required]
        public int BombPercentage { get; set; }
        
        public List<Tile>? Tiles { get; set; }
    }
}
