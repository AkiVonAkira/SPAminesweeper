using System;
namespace SPAmineseweeper.Models
{
    public class Tile
    {
        public int Id { get; set; }
        public int BoardId { get; set; } 
        public int X { get; set; } 
        public int Y { get; set; } 
        public bool IsMine { get; set; }
        public int AdjacentMines { get; set; }
        public bool IsRevealed { get; set; }
        public bool IsFlagged { get; set; }
    }
}
