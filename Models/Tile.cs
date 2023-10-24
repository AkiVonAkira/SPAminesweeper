using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPAmineseweeper.Models
{
    public class Tile
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Board")]
        public int BoardId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsMine { get; set; }
        public int AdjacentMines { get; set; }
        public bool IsRevealed { get; set; }
        public bool IsFlagged { get; set; }
        public virtual Board? Board { get; set; } // navigerings property till Board
    }
}
