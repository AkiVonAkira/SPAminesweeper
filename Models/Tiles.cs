using System;
namespace SPAmineseweeper.Models
{
    public class Tile
    {
        public int Id { get; set; }
        // [ForeignKey("Board")] is a data annotation that tells the database that the GameId property is a foreign key to the Game table.
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
